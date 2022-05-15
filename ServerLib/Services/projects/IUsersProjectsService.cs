////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib;
using SharedLib.Models;

namespace ServerLib
{
    /// <summary>
    /// Сервис работы с пользовательскими проектами
    /// </summary>
    public interface IUsersProjectsService
    {
        /// <summary>
        /// Получить мои проекты
        /// </summary>
        /// <param name="pagination">Настройка пагинации</param>
        /// <returns>Мои проекты</returns>
        public Task<FindUsersProjectsResponseModel> GetMyProjectsAsync(PaginationRequestModel pagination);

        public Task<UserProjectResponseModel> GetProjectAsync(int project_id);
    }
}
