using MetaLib.Models;
using Microsoft.AspNetCore.Authorization;

namespace CustomPolicyProvider;

public class MinimumLevelRequirement : IAuthorizationRequirement
{
    public AccessLevelsUsersEnum Level { get; private set; }

    public MinimumLevelRequirement(AccessLevelsUsersEnum level) { Level = level; }
}
