////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;

namespace SrvMetaApp.Repositories
{
    public interface IMailServiceInterface
    {
        /// <summary>
        /// Отправить пользователю уведомление на Email, для подтверждения операции
        /// </summary>
        public Task<bool> SendEmailRestoreUser(ConfirmationModelDb confirm_db);

        /// <summary>
        /// Отправка Email
        /// </summary>
        public Task SendEmailAsync(string email, string subject, string message, MimeKit.Text.TextFormat format);

        /// <summary>
        /// Отправить пользователю уведомление на Email, для подтверждения операции
        /// </summary>
        public Task<bool> SendEmailRegistrationUser(ConfirmationModelDb confirm_db);
    }
}
