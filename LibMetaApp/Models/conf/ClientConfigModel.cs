////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace LibMetaApp.Models
{
    public class ClientConfigModel
    {
        public string ApiHostName { get; set; } = "localhost";
        public int ApiHostPort { get; set; } = 5501;
        public string HttpSheme { get; set; } = "http";
        public string AllowedHosts { get; set; } = "*";
    }
}
