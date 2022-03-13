////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Newtonsoft.Json;

namespace LibMetaApp.Models
{
    public class AuthUserResultModel : ResultRequestModel
    {
        [JsonProperty("sessionMarker")]
        public SessionMarkerLiteModel SessionMarker { get; set; }
        //public override string ToString()
        //{
        //    return $"{(IsSuccess ? SessionMarker.Login : $"error auth: {Message}")}";
        //}
    }
}