////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace LibMetaApp.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Login), IsUnique = true)]
    [Index(nameof(AccessLevelUser))]
    public class UserModelDB : EntryCreatedModel
    {
        public UserModelDB() { }

        public UserModelDB(string name) : base(name) { }

        public UserModelDB(string name, AccessLevelsUsersEnum access_level) : base(name) { AccessLevelUser = access_level; }


        public string LastName { get; set; } = string.Empty;

        public string Login { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public AccessLevelsUsersEnum AccessLevelUser { get; set; } = AccessLevelsUsersEnum.Anonim;

        public ConfirmationUsersTypesEnum ConfirmationType { get; set; } = ConfirmationUsersTypesEnum.None;

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