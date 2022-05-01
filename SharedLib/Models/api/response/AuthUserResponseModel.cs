////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Newtonsoft.Json;

namespace SharedLib.Models
{
    /// <summary>
    /// Маркер созданной сессии (результат запроса)
    /// </summary>
    public class AuthUserResponseModel : ResponseBaseModel
    {
        /// <summary>
        /// Маркер сессии
        /// </summary>
        [JsonProperty("sessionMarker")]
        public SessionMarkerLiteModel SessionMarker { get; set; }
        //public override string ToString()
        //{
        //    return $"{(IsSuccess ? SessionMarker.Login : $"error auth: {Message}")}";
        //}
    }
}