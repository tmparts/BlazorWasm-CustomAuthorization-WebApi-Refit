////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace LibMetaApp
{
    public class SessionServiceLiteModel
    {
        public static string SessionTokenName => "token";
        public static string SessionLongTimeName => "session_long_time";
        public string GuidToken { get; set; } = string.Empty;
    }
}
