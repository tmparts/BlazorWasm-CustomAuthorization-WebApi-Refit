////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Newtonsoft.Json;

namespace MetaLib.Models
{
    public class SessionMarkerModel : SessionMarkerLiteModel
    {
        [JsonProperty("isParsed")]
        public bool IsParsed { get; private set; } = false;

        public bool IsLongTimeSession { get; set; }

        public SessionMarkerModel() { }
        public SessionMarkerModel(int id, string set_login, AccessLevelsUsersEnum set_access_level_user, string set_token, bool is_long_time_session)
        {
            Id = id;
            Login = set_login;
            AccessLevelUser = set_access_level_user;
            Token = set_token;
            IsLongTimeSession = is_long_time_session;
        }

        /// <summary>
        /// маркер сессии. например: 8|20|t1|user_login
        /// </summary>
        /// <param name="session_marker"> первое число - это идентификатор пользователя
        /// потом разделитель "|".
        /// затем - идентификатор уровня доступа.
        /// потом разделитель "|".
        /// затем признак длительности сессии: t1 - стандартная; t2 - длительная
        /// потом разделитель "|".
        /// затем логин.
        /// логин</param>
        public SessionMarkerModel(string session_marker)
        {
            session_marker = session_marker?.Trim() ?? string.Empty;
            if (session_marker.Length < 8)
            {
                return;
            }
            string[] segments = session_marker.Split("|");

            if (segments.Length != 4)
            {
                return;
            }

            if (!int.TryParse(segments[0], out int user_id))
            {
                return;
            }

            if (!int.TryParse(segments[1], out int access_level_user))
            {
                return;
            }

            Login = segments[3];

            if (string.IsNullOrEmpty(Login))
            {
                return;
            }

            Id = user_id;
            IsLongTimeSession = segments[2].ToLower() == "t2";
            AccessLevelUser = (AccessLevelsUsersEnum)access_level_user;
            IsParsed = true;
        }

        public static explicit operator string(SessionMarkerModel v)
        {
            return v.ToString();
        }

        public static explicit operator SessionMarkerModel(string v)
        {
            return new SessionMarkerModel(v);
        }

        /// <summary>
        /// Сформировать токен сессии пользователя
        /// </summary>
        /// <param name="set_access_level_user">уровень доступа пользователя</param>
        /// <param name="set_login">логин пользователя</param>
        /// <param name="is_long_time_session">Признак длительной сессии</param>
        /// <returns></returns>
        public static string GetSessionMarker(int id, AccessLevelsUsersEnum set_access_level_user, string set_login, bool is_long_time_session)
        {
            return $"{id}|{(int)set_access_level_user}|{(is_long_time_session ? "t2" : "t1")}|{set_login}";
        }

        public override string ToString()
        {
            return $"{Id}|{(int)AccessLevelUser}|{(IsLongTimeSession ? "t2" : "t1")}|{Login}";
        }
    }
}
