////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Newtonsoft.Json;

namespace MetaLib.Models
{
    public class SessionMarkerLiteModel
    {
        [JsonProperty("login")]
        public int Id { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; } = string.Empty;

        [JsonProperty("token")]
        public string Token { get; set; } = string.Empty;

        [JsonProperty("accessLevelUser")]
        public AccessLevelsUsersEnum AccessLevelUser { get; set; } = AccessLevelsUsersEnum.Anonim;

        public void Reload(int id, string set_login, AccessLevelsUsersEnum set_access_level_user, string set_token)
        {
            Id = id;
            Login = set_login;
            AccessLevelUser = set_access_level_user;
            Token = set_token;
        }

        public void Reload(SessionMarkerLiteModel new_marker)
        {
            Id = new_marker.Id;
            Login = new_marker.Login;
            AccessLevelUser = new_marker.AccessLevelUser;
            Token = new_marker.Token;
        }
    }
}
