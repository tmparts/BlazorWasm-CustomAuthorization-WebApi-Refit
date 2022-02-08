using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMetaApp.Models.enums
{
    /// <summary>
    /// Типы подтверждений действий пользователей
    /// </summary>
    public enum ConfirmationsTypesEnum
    {
        /// <summary>
        /// Подтверждение регистрации пользователя 
        /// </summary>
        RegistrationUser = 1,

        /// <summary>
        /// Подтверждение восстановления доступа к учётной записи
        /// </summary>
        RestoreUser = 2
    }
}
