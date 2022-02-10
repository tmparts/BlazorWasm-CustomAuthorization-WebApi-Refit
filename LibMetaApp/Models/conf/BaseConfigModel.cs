////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace LibMetaApp.Models
{
    public class BaseConfigModel
    {
        public HostConfigModel ApiConfig { get; set; } = new HostConfigModel() { Host = "localhost", Port = 5501, HttpSheme = "http" };
        public HostConfigModel ClientConfig { get; set; } = new HostConfigModel() { Host = "localhost", Port = 7222, HttpSheme = "http" };

        public ReCaptchaConfigClientModel ReCaptchaConfig { get; set; } = new ReCaptchaConfigClientModel();
    }
}
