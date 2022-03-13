////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Newtonsoft.Json;

namespace LibMetaApp.Models
{
    public class ResultRequestModel
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;
    }
}