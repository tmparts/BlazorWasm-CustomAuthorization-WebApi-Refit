////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

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
    }
}