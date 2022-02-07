////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace LibMetaApp.Models
{
    [Index(nameof(Email))]
    [Index(nameof(Login))]
    [Index(nameof(AccessLevelUser))]
    public class UserModelDB : EntryCreatedModel
    {
        public UserModelDB() { }

        public UserModelDB(int id, string name) : base(id, name) { }

        public UserModelDB(int id, string name, AccessLevelsUsersEnum access_level) : base(id, name) { AccessLevelUser = access_level; }
                

        public string LastName { get; set; } = string.Empty;

        public string Login { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public AccessLevelsUsersEnum AccessLevelUser { get; set; } = AccessLevelsUsersEnum.Anonim;

        public ConfirmationUsersTypesEnum VonfirmationType { get; set; } = ConfirmationUsersTypesEnum.None;

        public IEnumerable<GroupUserModelDB>? Groups { get; set; }

        public IEnumerable<ProjectModelDB>? Projects { get; set; }

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