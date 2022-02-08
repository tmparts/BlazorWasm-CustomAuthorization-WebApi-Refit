////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace LibMetaApp.Models
{
    public class ServerConfigModel : BaseConfigModel
    {
        /// <summary>
        /// Путь к папке для хранения файлов программы
        /// </summary>
        public string RootFolderName { get; set; } = "./storage-files";

        public DatabaseConfigModel DatabaseConfig { get; set; } = new DatabaseConfigModel();

        public HttpSessionCookieConfigModel CookiesConfig { get; set; } = new HttpSessionCookieConfigModel();

        public WebConfigModel WebConfig { get; set; } = new WebConfigModel();

        /// <summary>
        /// Настройки SMTP для отправки E-mail
        /// </summary>
        public SmtpConfigModel SmtpConfig { get; set; } = new SmtpConfigModel();

        public RedisConfigModel RedisConfig { get; set; } = new RedisConfigModel();

        public UserManageModel UserManageConfig { get; set; } = new UserManageModel();
    }
}