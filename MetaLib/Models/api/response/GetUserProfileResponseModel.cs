////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace MetaLib.Models
{
    public class GetUserProfileResponseModel : FindResponseModel
    {
        public UserMediumModel User { get; set; }
    }
}