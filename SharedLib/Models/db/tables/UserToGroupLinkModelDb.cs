////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib.Models
{
    /// <summary>
    /// Связи пользователей с группами
    /// </summary>
    [Index(nameof(UserId))]
    [Index(nameof(GroupId))]
    [Index(nameof(UserId), nameof(GroupId))]
    public class UserToGroupLinkModelDb : IdModel
    {
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Пользователь
        /// </summary>
        public UserModelDB User { get; set; }

        /// <summary>
        /// Идентификатор группы
        /// </summary>
        public int GroupId { get; set; }
        /// <summary>
        /// Группа пользователя
        /// </summary>
        public UserGroupModelDB Group { get; set; }
    }
}