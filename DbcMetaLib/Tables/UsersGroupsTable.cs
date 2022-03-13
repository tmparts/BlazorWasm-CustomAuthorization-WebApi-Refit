////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using DbcMetaLib.UsersGroups;
using LibMetaApp.Models;
using Microsoft.Extensions.Logging;
using SrvMetaApp;

namespace DbcMetaSqliteLib.UsersGroups
{
    /// <summary>
    /// Доступ к таблице групп пользователей базы данных SQLite
    /// </summary>
    public class UsersGroupsTable : IUsersGroupsTable
    {
        readonly MetaAppSqliteContext _db_context;
        readonly ILogger<UsersGroupsTable> _logger;

        public UsersGroupsTable(MetaAppSqliteContext set_db_context, ILogger<UsersGroupsTable> set_logger)
        {
            _db_context = set_db_context;
            _logger = set_logger;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db_context.SaveChangesAsync();
        }

        public async Task AddAsync(UserGroupModelDB user_group, bool auto_save = true)
        {
            await _db_context.AddAsync(user_group);
            if (auto_save)
                await _db_context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserGroupModelDB user_group, bool auto_save = true)
        {
            _db_context.Update(user_group);
            if (auto_save)
                await _db_context.SaveChangesAsync();
        }
    }
}
