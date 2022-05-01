////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib.Models;

namespace SharedLib
{
    /// <summary>
    /// Интерфейс доступа к проектам
    /// </summary>
    public interface IProjectsTable : SavingChanges
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
    }
}
