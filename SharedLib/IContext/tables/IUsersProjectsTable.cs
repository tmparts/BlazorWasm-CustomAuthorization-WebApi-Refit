////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib.Models;

namespace SharedLib
{
    /// <summary>
    /// Интерфейс доступа к проектам
    /// </summary>
    public interface IUsersProjectsTable : SavingChanges
    {
        /// <summary>
        /// Добавить новый проект в базу данных
        /// </summary>
        /// <param name="project">Объект проекта для добавления в БД</param>
        /// <param name="auto_save">Автоматически сохранить изменения в БД</param>
        public Task AddAsync(ProjectModelDB project, bool auto_save = true);

        /// <summary>
        /// Обновить объект проекта в БД
        /// </summary>
        /// <param name="project">Объект проекта для обновления в БД</param>
        /// <param name="auto_save">Автоматически сохранить изменения в БД</param>
        public Task UpdateAsync(ProjectModelDB project, bool auto_save = true);

        /// <summary>
        /// Удалить проект
        /// </summary>
        /// <param name="project_id">Идентификатор проекта</param>
        /// <param name="auto_save">Автоматически сохранить изменения в БД</param>
        public Task<bool> DeleteAsync(int project_id, bool auto_save = true);

        /// <summary>
        /// Получить проекты для пользователя
        /// </summary>
        /// <param name="user">Пользователь, для которого производится поиск</param>
        /// <param name="pagination">Настройки пагинатора</param>
        /// <returns>Набор проектов для пользователя</returns>
        public Task<ProjectsForUserResponseModel> GetProjectsForUserAsync((int Id, AccessLevelsUsersEnum AccessLevelUser) user, PaginationRequestModel pagination);

        /// <summary>
        /// Получить проект
        /// </summary>
        /// <param name="project_id">Идентификатор пользовательского проекта</param>
        /// <returns>Пользовательский проект</returns>
        public Task<ProjectModelDB?> GetProjectAsync(int project_id, bool include_users_data);
    }
}
