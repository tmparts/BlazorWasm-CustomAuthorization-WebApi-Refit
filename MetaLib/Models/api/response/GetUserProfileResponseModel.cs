////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace LibMetaApp.Models
{
    public class GetUserProfileResponseModel : FindResponseModel
    {
        public UserMediumModel User { get; set; }
    }
}