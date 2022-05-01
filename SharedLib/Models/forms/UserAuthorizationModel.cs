﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib.Models
{
    /// <summary>
    /// Объект пользователя для авторизация (создание сессии)
    /// </summary>
    public class UserAuthorizationModel : UserAuthorizationLiteModel
    {
        /// <summary>
        /// Чекбокс: запомнить меня
        /// </summary>
        public bool RememberMe { get; set; }
    }
}
