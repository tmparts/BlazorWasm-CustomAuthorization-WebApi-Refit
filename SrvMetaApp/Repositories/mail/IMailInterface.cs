////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;

namespace SrvMetaApp.Repositories
{
    public interface IMailInterface
    {
        /// <summary>
        /// Отправить пользователю уведомление на Email, для подтверждения операции
        /// </summary>
        public Task<bool> SendEmailRestoreUser(UserModelDB user);

        /// <summary>
        /// Отправка Email
        /// </summary>
        public Task SendEmailAsync(string email, string subject, string message, MimeKit.Text.TextFormat format);

        /// <summary>
        /// Отправить пользователю уведомление на Email, для подтверждения операции
        /// </summary>
        public Task<bool> SendEmailRegistrationUser(UserModelDB user_db, ConfirmationModelDb confirm_db);
    }
}
