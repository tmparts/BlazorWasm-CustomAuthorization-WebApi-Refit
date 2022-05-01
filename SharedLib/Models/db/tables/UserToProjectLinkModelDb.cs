﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib.Models
{
    /// <summary>
    /// Связи пользователей с проектами
    /// </summary>
    [Index(nameof(UserId))]
    [Index(nameof(ProjectId))]
    [Index(nameof(UserId), nameof(ProjectId))]
    public class UserToProjectLinkModelDb : IdModel
    {
        /// <summary>
        /// Иденификатор пользователя
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Идентификатор проекта
        /// </summary>
        public int ProjectId { get; set; }
    }
}