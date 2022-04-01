////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SrvMetaApp;

namespace MetaLib
{
    public static class ConfigureServices
    {
        public static void InitAccessMinLevelHandler(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, AccessMinLevelHandler>();

            services.AddAuthorizationCore(opts =>
            {
                opts.AddPolicy("AccessMinLevel" + AccessLevelsUsersEnum.Auth.ToString(),
                    policy => policy.Requirements.Add(new AccessMinLevelRequirement(AccessLevelsUsersEnum.Auth)));
                opts.AddPolicy("AccessMinLevel" + AccessLevelsUsersEnum.Confirmed.ToString(),
                    policy => policy.Requirements.Add(new AccessMinLevelRequirement(AccessLevelsUsersEnum.Confirmed)));
                opts.AddPolicy("AccessMinLevel" + AccessLevelsUsersEnum.Trusted.ToString(),
                    policy => policy.Requirements.Add(new AccessMinLevelRequirement(AccessLevelsUsersEnum.Trusted)));
                opts.AddPolicy("AccessMinLevel" + AccessLevelsUsersEnum.Manager.ToString(),
                    policy => policy.Requirements.Add(new AccessMinLevelRequirement(AccessLevelsUsersEnum.Manager)));
                opts.AddPolicy("AccessMinLevel" + AccessLevelsUsersEnum.Admin.ToString(),
                    policy => policy.Requirements.Add(new AccessMinLevelRequirement(AccessLevelsUsersEnum.Admin)));
                opts.AddPolicy("AccessMinLevel" + AccessLevelsUsersEnum.ROOT.ToString(),
                    policy => policy.Requirements.Add(new AccessMinLevelRequirement(AccessLevelsUsersEnum.ROOT)));
                //
                opts.DefaultPolicy = new AuthorizationPolicyBuilder()
                  .RequireAuthenticatedUser()
                  .Build();
            });
        }
    }
}
