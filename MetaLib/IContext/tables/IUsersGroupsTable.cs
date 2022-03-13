////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using DbcMetaLib.Confirmations;
using LibMetaApp.Models;

namespace DbcMetaLib.UsersGroups
{
    /// <summary>
    /// Интерфейс доступа к данным групп пользователей
    /// </summary>
    public interface IUsersGroupsTable : SavingChanges
    {
        
        public Task AddAsync(UserGroupModelDB user_group, bool auto_save = true);

        public Task UpdateAsync(UserGroupModelDB user_group, bool auto_save = true);
    }
}
