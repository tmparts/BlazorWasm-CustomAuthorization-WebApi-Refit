////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Newtonsoft.Json;

namespace SharedLib.Models
{
    /// <summary>
    /// Сессия пользователя
    /// </summary>
    public class SessionStorageModel
    {
        /// <summary>
        /// Токен/guid сессии
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// ДатаВремя старта сессии
        /// </summary>
        public DateTime DateTimeOfBirth { get; set; }

        /// <summary>
        /// IP адрес клиента на момент старта сессии
        /// </summary>
        public string ClientAddress { get; set; }

    }
}
