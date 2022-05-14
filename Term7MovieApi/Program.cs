using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.Cookie.Name = "Authentication";
});
builder.Services.AddSession(option =>
{
    option.Cookie.Name = "SessionId";
});
builder.Services.AddDistributedMemoryCache();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    builder.WebHost.ConfigureLogging((context, build) =>
    {
        //build.AddFile(builder.Configuration.GetSection("FileLogging"));
    });
}
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Use(async (context, next) =>
{
    string ip = context.Request.Headers["CF-CONNECTING-IP"].FirstOrDefault();
    app.Logger.LogInformation($"Request-from-ip:{ip};ConnectionId:{context.Connection.Id};Path:{context.Request.Path};Method:{context.Request.Method};Content-Type:{context.Request.ContentType};Content-Length:{context.Request.ContentLength}");
    await next.Invoke();
    string logMessage = $"Respone-for-ip:{ip};ConnectionId:{context.Connection.Id};;Path:{context.Request.Path};Status-Code:{context.Response.StatusCode};Content-Length:{context.Response.ContentLength}";
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
