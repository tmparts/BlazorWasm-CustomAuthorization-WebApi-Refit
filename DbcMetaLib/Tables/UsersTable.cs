////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using DbcMetaLib.Users;
using LibMetaApp.Models;
using MetaLib.Models.enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SrvMetaApp;

namespace DbcMetaSqliteLib.Users
{
    /// <summary>
    /// Доступ к таблице пользователей базы данных SQLite
    /// </summary>
    public class UsersTable : IUsersTable
    {
        readonly MetaAppSqliteContext _db_context;
        readonly ILogger<UsersTable> _logger;

        public UsersTable(MetaAppSqliteContext set_db_context, ILogger<UsersTable> set_logger)
        {
            _db_context = set_db_context;
            _logger = set_logger;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db_context.SaveChangesAsync();
        }

        public async Task AddAsync(UserModelDB user, bool auto_save = true)
        {
            await _db_context.AddAsync(user);
            if (auto_save)
                await _db_context.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserModelDB user, bool auto_save = true)
        {
            _db_context.Update(user);
            if (auto_save)
                await _db_context.SaveChangesAsync();
        }

        public async Task<bool> AnyByLoginOrEmailAsync(string login, string email)
        {
            return await _db_context.Users.AnyAsync(x => x.Login == login || x.Email == email);
        }

        public async Task<UserModelDB?> FirstOrDefaultByLoginAsync(string login)
        {
            return await _db_context.Users.FirstOrDefaultAsync(x => x.Login == login);
        }

        public async Task<UserModelDB?> FirstOrDefaultByEmailAsync(string email)
        {
            return await _db_context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
