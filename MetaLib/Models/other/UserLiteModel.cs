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
    }
}