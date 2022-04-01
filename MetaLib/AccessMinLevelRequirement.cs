////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models;
using Microsoft.AspNetCore.Authorization;

namespace SrvMetaApp
{
    public class AccessMinLevelRequirement : IAuthorizationRequirement
    {
        protected internal AccessLevelsUsersEnum LevelAccess { get; set; }

        public AccessMinLevelRequirement(AccessLevelsUsersEnum set_level_access)
        {
            LevelAccess = set_level_access;
        }
    }
}
