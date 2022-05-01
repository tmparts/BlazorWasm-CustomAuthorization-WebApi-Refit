////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib.Models;

namespace SharedLib
{
    /// <summary>
    /// Подтверждение действий пользователя (доступ к таблице базы данных)
    /// </summary>
    public interface IConfirmationsTable : SavingChanges
    {
        /// <summary>
        /// Поиск актуальной/непогашеной записи подтверждения действия пользователя
        /// </summary>
        /// <param name="confirm_id">Идентификатор пользователя-владельца подтверждения действия</param>
        /// <param name="include_user_data">Дополнительно загрузхить связанные данные</param>
        /// <returns>Объект подтверждения действия пользователя</returns>
        public Task<ConfirmationUserActionModelDb?> FirstOrDefaultActualAsync(string confirm_id, bool include_user_data = true);

        /// <summary>
        /// Добавть в таблицу базы данных новое подвтерждение действия
        /// </summary>
        /// <param name="confirmation">Подвтерждение действия</param>
        /// <param name="auto_save">Автоматическое сохранение в БД</param>
        public Task AddAsync(ConfirmationUserActionModelDb confirmation, bool auto_save = true);
        /// <summary>
        /// Обновить состояние актуальных подтвреждений действий пользователя. Если существуют схожие по смыслу с новым, то они будут деактивированы
        /// </summary>
        /// <param name="confirmation"></param>
        /// <param name="auto_save"></param>
        public Task ReNewAsync(ConfirmationUserActionModelDb confirmation, bool auto_save = true);

        /// <summary>
        /// Обновить объект подтверждения действия пользователя
        /// </summary>
        /// <param name="confirmation">Существующее подтверждение действия пользователя</param>
        /// <param name="auto_save">Автоматически сохранять данные в БД</param>
        public Task UpdateAsync(ConfirmationUserActionModelDb confirmation, bool auto_save = true);

        /// <summary>
        /// Удалить устаревшие записи журнала подтверждений действий пользователей
        /// </summary>
        /// <param name="auto_save">Автоматически сохранять данные в БД</param>
        public Task RemoveOutdatedRowsAsync(bool auto_save = true);
    }
}
