////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace MetaLib.Models
{
    public class BaseConfigModel
    {
        /// <summary>
        /// Настройки хоста API сервера
        /// </summary>
        public HostConfigModel ApiConfig { get; set; } = new HostConfigModel() { Host = "localhost", Port = 5501, HttpSheme = "http" };

        public HttpSessionCookieConfigModel CookiesConfig { get; set; } = new HttpSessionCookieConfigModel();

        /// <summary>
        /// Настройки хоста Blazor сервера
        /// </summary>
        public HostConfigModel ClientConfig { get; set; } = new HostConfigModel() { Host = "localhost", Port = 7222, HttpSheme = "http" };

        public ReCaptchaConfigClientModel ReCaptchaConfig { get; set; }
    }
}
