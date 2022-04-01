////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace MetaLib.Models
{
    public class SmtpConfigModel
    {
        public string[] EmailNotificationRecipients { get; set; } = new string[] { "ru.usa@mail.ru" };

        /// <summary>
        /// Публичное имя отправителя
        /// </summary>
        public string PublicName { get; set; } = "Public sender name";
        /// <summary>
        /// E-mail отправителя
        /// </summary>
        public string Email { get; set; } = "Email sender";
        /// <summary>
        /// Хост SMTP сервера
        /// </summary>
        public string Host { get; set; } = string.Empty;
        /// <summary>
        /// Порт хотса SMTP сервера
        /// </summary>
        public int Port { get; set; } = 465;
        /// <summary>
        /// Имя для входа/авторизации SMTP
        /// </summary>
        public string Login { get; set; } = string.Empty;
        /// <summary>
        /// Пароль для входа/авторизации SMTP
        /// </summary>
        public string Password { get; set; } = string.Empty;
        /// <summary>
        /// true, если клиент должен установить SSL-обернутое соединение с сервером; в противном случае false.
        /// </summary>
        public bool UseSsl { get; set; } = true;
        public bool IsEmptyConfig() => string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Login) || string.IsNullOrEmpty(Host);
    }
}
