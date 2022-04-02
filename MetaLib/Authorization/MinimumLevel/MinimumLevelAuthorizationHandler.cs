using System.Security.Claims;
using MetaLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace CustomPolicyProvider;

public class MinimumLevelAuthorizationHandler : AuthorizationHandler<MinimumLevelRequirement>
{
    private readonly ILogger<MinimumLevelAuthorizationHandler> _logger;

    public MinimumLevelAuthorizationHandler(ILogger<MinimumLevelAuthorizationHandler> logger)
    {
        _logger = logger;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumLevelRequirement requirement)
    {
        if (context.User.HasClaim(c => c.Type == ClaimTypes.Role))
        {
            AccessLevelsUsersEnum userRole = (AccessLevelsUsersEnum)Enum.Parse(typeof(AccessLevelsUsersEnum), context.User.FindFirst(c => c.Type == ClaimTypes.Role).Value);
            if (userRole >= requirement.Level)
            {
                context.Succeed(requirement);
            }
        }

        else
        {
            _logger.LogInformation("No Role claim present");
        }

        return Task.CompletedTask;
    }
}
