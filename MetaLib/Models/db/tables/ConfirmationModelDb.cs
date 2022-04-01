////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models.enums;
using Microsoft.EntityFrameworkCore;

namespace MetaLib.Models
{
    /// <summary>
    /// Подтверждение намерений пользователя
    /// </summary>
    [Index(nameof(ConfirmationType))]
    [Index(nameof(GuidConfirmation), IsUnique = true)]
    public class ConfirmationModelDb : EntryCreatedModel
    {
        public string GuidConfirmation { get; set; } = string.Empty;

        public int UserId { get; set; }
        public UserModelDB User { get; set; }

        public DateTime Deadline { get; set; }

        public ConfirmationsTypesEnum ConfirmationType { get; set; }

        /// <summary>
        /// Когда был подвтерждён (если был)
        /// </summary>
        public DateTime? ConfirmetAt { get; set; }

        public ConfirmationModelDb() { }

        public ConfirmationModelDb(UserModelDB user, ConfirmationsTypesEnum confirmation_type)
        {
            GuidConfirmation = Guid.NewGuid().ToString();
            UserId = user.Id;
            User = user;
            ConfirmationType = confirmation_type;
            Name = confirmation_type switch
            {
                ConfirmationsTypesEnum.RegistrationUser => $"Регистрация пользователя [login:'{user.Login}']",
                ConfirmationsTypesEnum.RestoreUser => $"Сброс пароля [login:'{user.Login}']",
                _ => throw new ArgumentOutOfRangeException(nameof(confirmation_type), "Тип подвтерждения действия пользователя не определён"),
            };
        }

        public ConfirmationModelDb(UserModelDB user, ConfirmationsTypesEnum confirmation_type, DateTime deadline) : this(user, confirmation_type)
        {
            Deadline = deadline;
        }
    }
}