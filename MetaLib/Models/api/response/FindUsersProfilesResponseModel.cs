////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace LibMetaApp.Models
{
    public class FindUsersProfilesResponseModel : FindResponseModel
    {
        public UserLiteModel[] Users { get; set; }
    }
}