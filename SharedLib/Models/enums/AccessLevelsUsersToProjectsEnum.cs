﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib.Models
{
    /// <summary>
    /// Уровни доступа пользователя к проекту
    /// </summary>
    public enum AccessLevelsUsersToProjectsEnum
    {
        /// <summary>
        /// Заблокирован
        /// </summary>
        [Display(Name = "Заблокирован", Description = "Заблокирован")]
        Blocked = -10,

        /// <summary>
        /// Чтение и просмотр
        /// </summary>
        Reader = 10,

        /// <summary>
        /// Редактирование объектов (без выдачи прав другим пользователям)
        /// </summary>
        Editor = 20,

        /// <summary>
        /// Полные права (в т.ч. выдача прав другим пользователям)
        /// </summary>
        Admin = 30
    }
}