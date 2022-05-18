////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib.Models;

namespace SharedLib
{
    /// <summary>
    /// Интерфейс доступа к ссылкам пользователей на проекты
    /// </summary>
    public interface ILinksProjectsTable : SavingChanges
    {
        /// <summary>
        /// Получить ссылки на проект
        /// </summary>
        /// <param name="project_id">Идентификатор проекта</param>
        /// <param name="include_user_data">Загрузить данные по пользователям</param>
        /// <returns>Сссылки на проект</returns>
        public Task<IEnumerable<UserToProjectLinkModelDb>> GetLinksUsersByProject(int project_id, bool include_user_data = true);
    }
}