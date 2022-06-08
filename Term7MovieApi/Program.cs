using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Term7MovieCore.Entities;
using Microsoft.AspNetCore.Identity;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieRepository.Repositories.Implement;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Term7MovieApi.Extensions;
using Term7MovieApi.Profiles;
using Term7MovieApi.Filters;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers(options => options.Filters.Add(typeof(ExceptionFilter)));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContext<AppDbContext>(o => o.UseSqlServer(config.GetConnectionString("FCinemaConnection"), b => b.MigrationsAssembly("Term7MovieApi")));

builder.Services.AddIdentity<User, Role>(options => options.SignIn.RequireConfirmedPhoneNumber = false)
                    .AddRoles<Role>()
                    .AddRoleManager<RoleManager<Role>>()
                    .AddUserManager<UserManager<User>>()
                    .AddSignInManager<SignInManager<User>>()
                    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

// inject services from Term7MovieService.Services
builder.Services.InjectProjectServices();

// create instance for config in appsettings.json
builder.Services.ConfigureOptions(config);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var jwtSection = config.GetSection("Jwt");
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"])),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.ConfigureAuthorization();

builder.Services.AddDistributedRedisCache(option =>
{
    option.Configuration = builder.Configuration.GetConnectionString("Redis");

});
builder.Services.AddCors(option =>
{
    option.AddPolicy("Default", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddSwaggerGen();
builder.Services.AddDistributedMemoryCache();

if (builder.Environment.IsProduction())
{
    //builder.Logging.ClearProviders();
    builder.WebHost.ConfigureLogging((context, build) =>
    {
        build.AddFile(builder.Configuration.GetSection("FileLogging"));
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo");
    });
}
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
app.UseCors("Default");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Use(async (context, next) =>
{
    string ip = context.Request.Headers["CF-CONNECTING-IP"].FirstOrDefault();
    app.Logger.LogInformation($"{DateTime.UtcNow} | Request-from-ip:{ip};ConnectionId:{context.Connection.Id};Path:{context.Request.Path};Method:{context.Request.Method};Content-Type:{context.Request.ContentType};Content-Length:{context.Request.ContentLength}");
#if DEBUG
    string headers = String.Empty;
    foreach (var key in context.Request.Headers.Keys)
        headers += key + ": " + context.Request.Headers[key] + Environment.NewLine;
    app.Logger.LogInformation(headers);
#endif
    await next.Invoke();
    string logMessage = $"{DateTime.UtcNow} | Respone-for-ip:{ip};ConnectionId:{context.Connection.Id};;Path:{context.Request.Path};Status-Code:{context.Response.StatusCode};Content-Length:{context.Response.ContentLength}";
    if (context.Response.StatusCode < 400)
    {
        app.Logger.LogInformation(logMessage);
    }
    else if (context.Response.StatusCode < 500)
    {
        app.Logger.LogWarning(logMessage);
    }
    else
    {
        app.Logger.LogError(logMessage);
    }
});

app.Run();
