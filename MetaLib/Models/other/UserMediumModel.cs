////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace LibMetaApp.Models
{
    public class UserMediumModel : UserLiteModel
    {
        public UserMediumModel() { }

        public UserMediumModel(string name) : base(name) { }

        public UserMediumModel(string name, AccessLevelsUsersEnum access_level) : base(name) { AccessLevelUser = access_level; }

        public UserMediumModel(string name, AccessLevelsUsersEnum access_level, ConfirmationUsersTypesEnum confirmation_type) : base(name) { AccessLevelUser = access_level; ConfirmationType = confirmation_type; }

        public IEnumerable<UserGroupModelDB>? Groups { get; set; }

        public IEnumerable<ProjectModelDB>? Projects { get; set; }
    }
}