////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace MetaLib.Models
{
    public class FindUsersProfilesResponseModel : FindResponseModel
    {
        public UserLiteModel[] Users { get; set; }
    }
}