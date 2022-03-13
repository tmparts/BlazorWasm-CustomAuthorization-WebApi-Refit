////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using DbcMetaLib.Confirmations;
using LibMetaApp.Models;
using MetaLib.Models.enums;

namespace DbcMetaLib.Users
{
    /// <summary>
    /// Интерфейс доступа к данным пользователей
    /// </summary>
    public interface IUsersTable : SavingChanges
    {
        public Task<bool> AnyByLoginOrEmailAsync(string login, string email);

        public Task<UserModelDB?> FirstOrDefaultByLoginAsync(string login);

        public Task<UserModelDB?> FirstOrDefaultByEmailAsync(string email);

        public Task AddAsync(UserModelDB user, bool auto_save = true);

        public Task UpdateAsync(UserModelDB user, bool auto_save = true);
    }
}
