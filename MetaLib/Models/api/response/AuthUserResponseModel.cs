////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Newtonsoft.Json;

namespace MetaLib.Models
{
    public class AuthUserResponseModel : ResponseBaseModel
    {
        [JsonProperty("sessionMarker")]
        public SessionMarkerLiteModel SessionMarker { get; set; }
        //public override string ToString()
        //{
        //    return $"{(IsSuccess ? SessionMarker.Login : $"error auth: {Message}")}";
        //}
    }
}