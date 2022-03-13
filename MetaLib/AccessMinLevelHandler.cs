////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SrvMetaApp
{
    public class AccessMinLevelHandler : AuthorizationHandler<AccessMinLevelRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AccessMinLevelRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == ClaimTypes.Role))
            {
                AccessLevelsUsersEnum userRole = (AccessLevelsUsersEnum)Enum.Parse(typeof(AccessLevelsUsersEnum), context.User.FindFirst(c => c.Type == ClaimTypes.Role).Value);
                if (userRole >= requirement.LevelAccess)
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}
