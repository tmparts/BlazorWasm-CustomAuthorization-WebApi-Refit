////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib.Models;

namespace SharedLib
{
    /// <summary>
    /// Доступ к таблице групп пользователей базы данных
    /// </summary>
    public interface IUsersGroupsTable : SavingChanges
    {
        /// <summary>
        /// Добавить новую группу пользователей
        /// </summary>
        /// <param name="user_group">Наименование группы пользователей</param>
        /// <param name="auto_save">Автоматически сохранить изменения в БД</param>
        public Task AddAsync(UserGroupModelDB user_group, bool auto_save = true);

        /// <summary>
        /// Обновить группу пользователей в БД
        /// </summary>
        /// <param name="user_group">Группа пользователдей для обновления</param>
        /// <param name="auto_save">Автоматически сохранить изменения в БД</param>
        public Task UpdateAsync(UserGroupModelDB user_group, bool auto_save = true);
    }
}
