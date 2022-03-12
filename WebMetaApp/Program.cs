////////////////////////////////////////////////
// � https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Blazored.LocalStorage;
using LibMetaApp;
using LibMetaApp.Models;
using LibMetaApp.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Newtonsoft.Json;
using Refit;
using System.Net.Http.Headers;
using WebMetaApp;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddScoped<ISessionLocalStorage, SessionLocalStorage>();

builder.Services.AddBlazoredLocalStorage();

SessionMarkerLiteModel marker = new SessionMarkerLiteModel() { AccessLevelUser = AccessLevelsUsersEnum.Anonim, Login = string.Empty, Token = string.Empty };
builder.Services.AddScoped<SessionMarkerLiteModel>(sp => marker);

#region Config

HttpClient http = new HttpClient()
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
};
http.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
{
    NoCache = true
};

// ClientConfig
HttpResponseMessage? response = await http.GetAsync("clientconfig.json");
Stream? stream = await response.Content.ReadAsStreamAsync();
IConfigurationRoot? config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();
ClientConfigModel? conf = new ClientConfigModel();
config.Bind(conf);

http = new HttpClient()
{
    BaseAddress = new Uri(conf.ApiConfig.ToString())
};
http.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
{
    NoCache = true
};

response = await http.GetAsync("api/ClientConfig");
stream = await response.Content.ReadAsStreamAsync();
IConfigurationRoot? remote_config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();
ClientConfigModel? remote_conf = new ClientConfigModel();
remote_config.Bind(remote_conf);
await stream.DisposeAsync();

conf.ReCaptchaConfig = remote_conf.ReCaptchaConfig;

builder.Services.AddSingleton<ClientConfigModel>(sp => conf);

builder.Services.AddScoped(sp => http);

#endregion

builder.Services.AddScoped<IUserAuthService, UserAuthService>();

builder.Services.AddRefitClient<IUsersAuthApi>()
        .ConfigureHttpClient(c => c.BaseAddress = new Uri($"{conf.ApiConfig.HttpSheme}://{conf.ApiConfig.Host}:{conf.ApiConfig.Port}/"))
        .AddHttpMessageHandler(provider => new RefitHeadersDelegatingHandler(marker))
        .SetHandlerLifetime(TimeSpan.FromMinutes(2));

builder.Services.InitAccessMinLevelHandler();

builder.Logging.SetMinimumLevel(LogLevel.Trace);
//builder.Logging.AddProvider(new CustomLoggingProvider());

WebAssemblyHost WebHost = builder.Build();

ISessionLocalStorage SessionLocalStorage = WebHost.Services.GetService<ISessionLocalStorage>();
SessionMarkerLiteModel set_marker = await SessionLocalStorage.ReadSessionAsync();
//marker.Reload(set_marker);

http.DefaultRequestHeaders.Add(GlobalStaticConstants.SESSION_TOKEN_NAME, set_marker.Token);
response = await http.GetAsync("api/UsersAuthorization");
string current_session_raw = await response.Content.ReadAsStringAsync();
SessionReadResultModel? session_token_online_object = JsonConvert.DeserializeObject<SessionReadResultModel>(current_session_raw);
if (session_token_online_object.SessionMarker != null)
    marker.Reload(session_token_online_object.SessionMarker);
http.DefaultRequestHeaders.Clear();

response.Dispose();

await WebHost.RunAsync();
