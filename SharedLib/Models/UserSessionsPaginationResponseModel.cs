////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib.Models
{
    /// <summary>
    /// Перечень сессий пользователей (результат запроса с пагинацией)
    /// </summary>
    public class UserSessionsPaginationResponseModel : PaginationResponseModel
    {
        /// <summary>
        /// Перечень сессий пользователей (результат запроса)
        /// </summary>
        public UserSessionModel[] Sessions { get; set; } = Array.Empty<UserSessionModel>();
    }
}
