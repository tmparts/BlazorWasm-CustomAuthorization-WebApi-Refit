////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace LibMetaApp.Models
{
    public class HostConfigModel
    {
        public string Host { get; set; } = "localhost";
        public int Port { get; set; } = 5501;
        public string HttpSheme { get; set; } = "http";
        public override string ToString() => $"{HttpSheme}://{Host}{(Port == 80 ? "" : $":{Port}")}";

        public string GetFullUrl(string patch) => $"{ToString()}/{(patch.StartsWith("/") ? patch[1..] : patch)}";
    }
}
