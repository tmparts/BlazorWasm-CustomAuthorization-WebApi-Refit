////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models;

namespace SrvMetaApp.Repositories
{
    public interface IMailServiceInterface
    {
        /// <summary>
        /// Отправка Email
        /// </summary>
        public Task SendEmailAsync(string email, string subject, string message, MimeKit.Text.TextFormat format);

        /// <summary>
        /// Отправить пользователю уведомление на Email, для подтверждения операции
        /// </summary>
        public Task<bool> SendUserConfirmationEmail(ConfirmationModelDb confirm_db);
    }
}
