////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace MetaLib.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Login), IsUnique = true)]

    [Index(nameof(ConfirmationType))]
    [Index(nameof(AccessLevelUser))]
    public class UserModelDB : UserMediumModel
    {
        public UserModelDB() { }

        public UserModelDB(string name) : base(name) { }

        public UserModelDB(string name, AccessLevelsUsersEnum access_level) : base(name) { AccessLevelUser = access_level; }

        public string PasswordHash { get; set; } = string.Empty;

        public static explicit operator UserModelDB(UserRegistrationModel v)
        {
            return new UserModelDB()
            {
                AccessLevelUser = AccessLevelsUsersEnum.Auth,
                Email = v.Email,
                Login = v.Login,
                Name = v.PublicName,
                PasswordHash = GlobalUtils.CalculateHashString(v.Password)
            };
        }
    }
}