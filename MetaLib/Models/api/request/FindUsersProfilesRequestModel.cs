////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace MetaLib.Models
{
    public class FindUsersProfilesRequestModel : PaginationRequestModel
    {
        public FindTextModel FindLogin { get; set; }
        public IEnumerable<AccessLevelsUsersEnum> AccessLevelsUsers { get; set; }
        public IEnumerable<ConfirmationUsersTypesEnum> ConfirmationUsersTypes { get; set; }
        public IEnumerable<int>? Groups { get; set; }

        public IEnumerable<int>? Projects { get; set; }
    }
}
