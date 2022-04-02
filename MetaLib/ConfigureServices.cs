////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using CustomPolicyProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

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
    }
}
