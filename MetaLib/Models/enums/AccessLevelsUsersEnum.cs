﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace MetaLib.Models
{
    public enum AccessLevelsUsersEnum
    {
        Anonim = -1,

        /// <summary>
        /// Заблокирован
        /// </summary>
        [Display(Name = "Заблокирован", Description = "Заблокирован")]
        Blocked = 0,

        /// <summary>
        /// Зарегистрированный (но НЕ подтверждённый)
        /// </summary>
        [Display(Name = "Зарегистрированый", Description = "Рядовой зарегистрированный пользователь (не подтверждённый)")]
        Auth = 10,

        /// <summary>
        /// Подтверждён (подтвердил по Email и/или Telegram)
        /// </summary>
        [Display(Name = "Проверенный", Description = "Подтверждённый пользователь (подтвердил по Email и/или Telegram)")]
        Confirmed = 20,

        /// <summary>
        /// Привилегированный
        /// </summary>
        [Display(Name = "Привилегированный", Description = "Особые разрешения, но не администрация")]
        Trusted = 30,

        /// <summary>
        /// 4.Менеджер (управляющий/модератор)
        /// </summary>
        [Display(Name = "Менеджер/Модератор", Description = "Младший администратор")]
        Manager = 40,

        /// <summary>
        /// Администратор
        /// </summary>
        Admin = 50,

        /// <summary>
        /// Владелец (суперпользователь)
        /// </summary>
        [Display(Name = "ROOT/Суперпользователь")]
        ROOT = 60
    }
}