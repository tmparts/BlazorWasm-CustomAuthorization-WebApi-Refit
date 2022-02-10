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
using Refit;
using System.Net.Http.Headers;
using WebMetaApp;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var http = new HttpClient()
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
};
http.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
{
    NoCache = true
};

//builder.Services.AddScoped(sp => http);

SessionMarkerLiteModel marker = new SessionMarkerLiteModel() { AccessLevelUser = AccessLevelsUsersEnum.Anonim, Login = string.Empty, Token = string.Empty };
builder.Services.AddScoped<SessionMarkerLiteModel>(sp => marker);

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<CustomAuthStateProvider>());
builder.Services.AddScoped<ISessionLocalStorage, SessionLocalStorage>();
builder.Services.AddBlazoredLocalStorage();

using HttpResponseMessage? response = await http.GetAsync("clientconfig.json");
using Stream? stream = await response.Content.ReadAsStreamAsync();
IConfigurationRoot? config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();
ClientConfigModel? conf = new ClientConfigModel();
config.Bind(conf);
builder.Services.AddSingleton<ClientConfigModel>(sp => conf);

builder.Services.AddScoped<IUserAuthService, UserAuthService>();

builder.Services.AddRefitClient<IUsersAuthApi>()
        .ConfigureHttpClient(c => c.BaseAddress = new Uri($"{conf.ApiConfig.HttpSheme}://{conf.ApiConfig.Host}:{conf.ApiConfig.Port}/"))
        .AddHttpMessageHandler(provider => new RefitHeadersDelegatingHandler(marker))
        .SetHandlerLifetime(TimeSpan.FromMinutes(2));

builder.Services.InitAccessMinLevelHandler();
await builder.Build().RunAsync();
