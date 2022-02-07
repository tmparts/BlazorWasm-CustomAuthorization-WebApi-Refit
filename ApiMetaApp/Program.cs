////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using ApiMetaApp;
using SrvMetaApp.Repositories;
using SrvMetaApp;
using LibMetaApp.Models;
using SrvMetaApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;
using LibMetaApp;
using SrvMetaApp.Repositories.mail;

Logger logger = LogManager.Setup().LoadConfigurationFromFile().GetCurrentClassLogger();
logger.Info("init main");

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("serverconfig.json");
ServerConfigModel? conf = new ServerConfigModel();
builder.Configuration.Bind(conf);
builder.Services.Configure<ServerConfigModel>(builder.Configuration);

builder.WebHost.UseKestrel(options =>
{
    options.Limits.MaxConcurrentConnections = conf.WebConfig.MaxConcurrentConnections;
    options.Limits.MaxRequestBodySize = conf.WebConfig.MaxRequestBodySize;
    options.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(conf.WebConfig.KeepAliveTimeout);
    options.Limits.MaxRequestLineSize = conf.WebConfig.MaxRequestLineSize;
    options.Limits.MinRequestBodyDataRate = new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
    options.Limits.MinResponseDataRate = new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));

    switch (conf.WebConfig.AllowedHosts.Trim().ToLower())
    {
        case "broadcast":
            options.Listen(IPAddress.Broadcast, conf.WebConfig.IpPort);
            break;
        case "loopback":
            options.Listen(IPAddress.Loopback, conf.WebConfig.IpPort);
            break;
        case "None":
            options.Listen(IPAddress.None, conf.WebConfig.IpPort);
            break;
        default:
            options.Listen(IPAddress.Any, conf.WebConfig.IpPort);
            break;
    }
});

builder.Services.AddScoped<MetaAppContextDB>();
builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<RedisUtil>();

// Add services to the container.
builder.Services.AddScoped<IUsersRepositoryInterface, UsersRepository>();
builder.Services.AddScoped<IMailInterface, MailRepository>();

builder.Services.InitAccessMinLevelHandler();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/UsersAuthorization";//api/<UsersController>/ReturnUrl=/api/UsersAuthorization
        options.AccessDeniedPath = "/AccessDenied";
        options.LogoutPath = "/Logout";
    });

builder.Services.AddDistributedMemoryCache();
builder.Services.AddMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".MyApp.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});

builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "DesignerMetaApp API",
        Description = "An ASP.NET Core Web API for Design metadata app",
        TermsOfService = new Uri("https://github.com/badhitman/DesignerMetaApp/blob/main/TERMS.md"),
        Contact = new OpenApiContact
        {
            Name = "my Contact",
            Url = new Uri("https://github.com/badhitman/DesignerMetaApp/blob/main/CONTACT.md")
        },
        License = new OpenApiLicense
        {
            Name = "License",
            Url = new Uri("https://github.com/badhitman/DesignerMetaApp/blob/main/LICENSE")
        }
    });
});

// NLog: Setup NLog for Dependency injection
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();

try
{
    WebApplication app = builder.Build();

    app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin =>
                {
                    return conf.WebConfig.ClientOrignsCORS.Contains(origin);
                }) // allow any origin
                .AllowCredentials()); // allow credentials

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });

    app.UseHttpsRedirection();

    app.UseMiddleware<PassageMiddleware>();
    app.UseAuthentication();    // аутентификация
    app.UseAuthorization();     // авторизация

    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}