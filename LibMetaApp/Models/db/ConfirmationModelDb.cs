////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models.enums;
using Microsoft.EntityFrameworkCore;

namespace LibMetaApp.Models
{
    /// <summary>
    /// Подтверждение намерений пользователя
    /// </summary>
    [Index(nameof(ConfirmationType))]
    [Index(nameof(Guid), IsUnique = true)]
    public class ConfirmationModelDb : EntryCreatedModel
    {
        public ConfirmationModelDb() { }
        public ConfirmationModelDb(string name, UserModelDB user, string guid, ConfirmationsTypesEnum confirm_type, DateTime deadline) : base(name)
        {
            Guid = guid;
            ConfirmationType = confirm_type;
            User = user;
            Deadline = deadline;
        }

        public string Guid { get; set; } = string.Empty;

        public UserModelDB User { get; set; }

        public DateTime Deadline { get; set; }

        public ConfirmationsTypesEnum ConfirmationType { get; set; }

        /// <summary>
        /// Когда был подвтерждён (если был)
        /// </summary>
        public DateTime? ConfirmetAt { get; set; }
    }
}