////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Newtonsoft.Json;

namespace LibMetaApp.Models
{
    public class SessionMarkerModel : SessionMarkerLiteModel
    {
        [JsonProperty("isParsed")]
        public bool IsParsed { get; private set; } = false;

        public bool IsLongTimeSession { get; set; }

        public SessionMarkerModel() { }
        public SessionMarkerModel(string set_login, AccessLevelsUsersEnum set_access_level_user, string set_token, bool is_long_time_session)
        {
            Login = set_login;
            AccessLevelUser = set_access_level_user;
            Token = set_token;
            IsLongTimeSession = is_long_time_session;
        }

        /// <summary>
        /// маркер сессии. например: 020|t1|user_login
        /// </summary>
        /// <param name="session_marker">первые три символа - это идентификатор уровня доступа.
        /// потом разделитель "|".
        /// затем признак длительности сессии: t1 - стандартная; t2 - длительная
        /// потом разделитель "|".
        /// затем логин.
        /// логин</param>
        public SessionMarkerModel(string session_marker)
        {
            if (string.IsNullOrWhiteSpace(session_marker))
            {
                return;
            }
            session_marker = session_marker.Trim();
            if (session_marker.Length < 8)
            {
                return;
            }
            if (int.TryParse(session_marker[..3], out int access_level_user))
            {
                AccessLevelUser = (AccessLevelsUsersEnum)access_level_user;
            }

            IsLongTimeSession = session_marker.Substring(4, 2).ToLower() == "t2";

            Login = session_marker[7..];

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
        public static string GetSessionMarker(AccessLevelsUsersEnum set_access_level_user, string set_login, bool is_long_time_session)
        {
            return $"{((int)set_access_level_user):D3}|{(is_long_time_session ? "t2" : "t1")}|{set_login}";
        }

        public override string ToString()
        {
            return $"{((int)AccessLevelUser):D3}|{(IsLongTimeSession ? "t2" : "t1")}|{Login}";
        }
    }
}
