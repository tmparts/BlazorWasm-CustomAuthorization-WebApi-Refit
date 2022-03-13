////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;

namespace DbcMetaLib.Confirmations
{
    public interface IConfirmationsDb : SavingChanges
    {
        /// <summary>
        /// Поиск актуальной/непогашеной записи подтверждения действия пользователя
        /// </summary>
        /// <param name="confirm_id"></param>
        public Task<ConfirmationModelDb?> FirstOrDefaultActualAsync(string confirm_id, bool include_user_data = true);

        public Task AddAsync(ConfirmationModelDb confirmation, bool auto_save = true);

        public Task UpdateAsync(ConfirmationModelDb confirmation, bool auto_save = true);

        /// <summary>
        /// Удалить устаревшие записи журнала подтверждений действий пользователей
        /// </summary>
        public Task RemoveOutdatedRowsAsync(bool auto_save = true);
    }
}
