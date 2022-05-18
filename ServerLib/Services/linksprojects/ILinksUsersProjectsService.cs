////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib;
using SharedLib.Models;

namespace ServerLib
{
    /// <summary>
    /// Сервис работы с ссылками пользователей на проекты
    /// </summary>
    public interface ILinksUsersProjectsService
    {
        /// <summary>
        /// Получить ссылки пользователей на проект
        /// </summary>
        /// <param name="project_id">Идентификатор проекта</param>
        /// <returns>Ссылки пользователей на проект</returns>
        public Task<GetLinksProjectsResponseModel> GetLinksUsersByProject(int project_id);
    }
}
