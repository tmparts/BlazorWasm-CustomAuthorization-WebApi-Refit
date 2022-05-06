////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib.Models;

namespace SrvMetaApp.Repositories
{
    /// <summary>
    /// Сервис отправки Email
    /// </summary>
    public interface IMailProviderService
    {
        /// <summary>
        /// Отправка Email
        /// </summary>
        public Task SendEmailAsync(string email, string subject, string message, MimeKit.Text.TextFormat format);

        /// <summary>
        /// Отправить пользователю уведомление на Email, для подтверждения операции
        /// </summary>
        /// <param name="confirm_db">Объект подтверждения действия пользователя</param>
        public Task<bool> SendUserConfirmationEmail(ConfirmationUserActionModelDb confirm_db);
    }
}
