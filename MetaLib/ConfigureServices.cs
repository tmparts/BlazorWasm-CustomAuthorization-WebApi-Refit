////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using CustomPolicyProvider;
using MetaLib.ClientServices.refit;
using MetaLib.ClientServices.refit.users;
using MetaLib.ClientServices.refit.users.auth;
using MetaLib.Models;
using MetaLib.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace MetaLib
{
    public static class ConfigureServices
    {
        public static void InitAccessMinLevelHandler(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationPolicyProvider, MinimumLevelPolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, MinimumLevelAuthorizationHandler>();

            services.AddAuthorizationCore(opts =>
            {
                opts.DefaultPolicy = new AuthorizationPolicyBuilder()
                  .RequireAuthenticatedUser()
                  .Build();
            });
        }

        public static void InitRefit(this IServiceCollection services, ClientConfigModel conf, SessionMarkerLiteModel marker, TimeSpan handler_lifetime)
        {
            services.AddRefitClient<IUsersAuthRefitModel>()
                .ConfigureHttpClient(c => c.BaseAddress = conf.ApiConfig.Url)
                .AddHttpMessageHandler(provider => new RefitHeadersDelegatingHandler(marker))
                .SetHandlerLifetime(handler_lifetime);

            services.AddRefitClient<IUsersProfilesRefitModel>()
                .ConfigureHttpClient(c => c.BaseAddress = conf.ApiConfig.Url)
                .AddHttpMessageHandler(provider => new RefitHeadersDelegatingHandler(marker))
                .SetHandlerLifetime(handler_lifetime);
            //
            services.AddScoped<IUsersProfilesRefitService, UsersProfilesRefitService>();
            services.AddScoped<IUsersAuthRefitService, UsersAuthRefitService>();

            services.AddScoped<IUsersAuthRefitEngine, UsersAuthRefitEngine>();
            services.AddScoped<IUsersProfilesRefitEngine, UsersProfilesRefitEngine>();
        }
    }
}
