////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace MetaLib.Models
{
    public class UserLiteModel : EntryCreatedModel
    {
        public UserLiteModel() { }

        public UserLiteModel(string name) : base(name) { }

        public UserLiteModel(string name, AccessLevelsUsersEnum access_level) : base(name) { AccessLevelUser = access_level; }

        public UserLiteModel(string name, AccessLevelsUsersEnum access_level, ConfirmationUsersTypesEnum confirmation_type) : base(name) { AccessLevelUser = access_level; ConfirmationType = confirmation_type; }

        public string Login { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public AccessLevelsUsersEnum AccessLevelUser { get; set; } = AccessLevelsUsersEnum.Anonim;

        public ConfirmationUsersTypesEnum ConfirmationType { get; set; } = ConfirmationUsersTypesEnum.None;

        public static bool operator ==(UserLiteModel user1, UserLiteModel user2)
        {
            return user1.Id == user2.Id
                && user1.AccessLevelUser == user2.AccessLevelUser
                && user1.Email == user2.Email
                && user1.ConfirmationType == user2.ConfirmationType
                && user1.Login == user2.Login
                && user1.Name == user2.Name;
        }
        public static bool operator !=(UserLiteModel user1, UserLiteModel user2) => !(user1 == user2);

        public static bool operator ==(UserLiteModel user1, UserMediumModel user2)
        {
            return user1.Id == user2.Id
                && user1.AccessLevelUser == user2.AccessLevelUser
                && user1.Email == user2.Email
                && user1.ConfirmationType == user2.ConfirmationType
                && user1.Login == user2.Login
                && user1.Name == user2.Name;
        }
        public static bool operator !=(UserLiteModel user1, UserMediumModel user2) => !(user1 == user2);
    }
}