using MetaLib.Models;
using Microsoft.AspNetCore.Authorization;

namespace CustomPolicyProvider;

public class MinimumLevelAuthorizeAttribute : AuthorizeAttribute
{
    string POLICY_PREFIX = MinimumLevelPolicyProvider.POLICY_PREFIX;

    public MinimumLevelAuthorizeAttribute(AccessLevelsUsersEnum level) => Level = level;

    public AccessLevelsUsersEnum Level
    {
        get
        {
            if (AccessLevelsUsersEnum.TryParse(Policy.AsSpan(POLICY_PREFIX.Length), out AccessLevelsUsersEnum level))
            {
                return level;
            }
            return default;
        }
        set
        {
            Policy = $"{POLICY_PREFIX}{value}";
        }
    }
}
