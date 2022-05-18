////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Newtonsoft.Json;

namespace SharedLib.Models
{
    /// <summary>
    /// Профиль пользователя (результат запроса)
    /// </summary>
    public class GetUserProfileResponseModel : ResponseBaseModel
    {
        /// <summary>
        /// Профиль пользователя
        /// </summary>
        public UserMediumModel User { get; set; }

        /// <summary>
        /// Сессии пользователя
        /// </summary>
        public IEnumerable<UserSessionModel> Sessions { get; set; }
    }
}