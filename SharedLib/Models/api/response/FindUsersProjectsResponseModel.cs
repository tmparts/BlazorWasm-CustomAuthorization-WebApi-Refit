////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib.Models
{
    /// <summary>
    /// Профили пользователей (результат запроса/поиска)
    /// </summary>
    public class FindUsersProjectsResponseModel : ResponseBaseModel
    {
        /// <summary>
        /// Проекты пользователей
        /// </summary>
        public ProjectsForUserResponseModel Projects { get; set; }
    }
}