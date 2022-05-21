using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContext<DbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

builder.Services.AddDistributedRedisCache(option =>
{
    option.Configuration = builder.Configuration.GetConnectionString("Redis");
    
});

builder.Services.AddSwaggerGen();
builder.Services.AddDistributedMemoryCache();

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
else
{
    builder.WebHost.ConfigureLogging((context, build) =>
    {
        build.AddFile(builder.Configuration.GetSection("FileLogging"));
    });
}
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});
app.UseStaticFiles();

app.UseSession();
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
