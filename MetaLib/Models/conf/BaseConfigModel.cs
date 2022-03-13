﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace LibMetaApp.Models
{
    public class BaseConfigModel
    {
        /// <summary>
        /// Настройки хоста API сервера
        /// </summary>
        public HostConfigModel ApiConfig { get; set; } = new HostConfigModel() { Host = "localhost", Port = 5501, HttpSheme = "http" };

        /// <summary>
        /// Настройки хоста Blazor сервера
        /// </summary>
        public HostConfigModel ClientConfig { get; set; } = new HostConfigModel() { Host = "localhost", Port = 7222, HttpSheme = "http" };

        public ReCaptchaConfigClientModel ReCaptchaConfig { get; set; }
    }
}