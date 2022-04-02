using MetaLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace CustomPolicyProvider;

public class MinimumLevelPolicyProvider : IAuthorizationPolicyProvider
{
    public static string POLICY_PREFIX => "MinimumLevel";
    public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

    public MinimumLevelPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();

    public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase) &&
            AccessLevelsUsersEnum.TryParse(policyName.Substring(POLICY_PREFIX.Length), out AccessLevelsUsersEnum level))
        {
            AuthorizationPolicyBuilder? policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new MinimumLevelRequirement(level));
            return Task.FromResult(policy.Build());
        }

        return FallbackPolicyProvider.GetPolicyAsync(policyName);
    }
}
