////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using DbcMetaLib.Confirmations;
using LibMetaApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SrvMetaApp;

namespace DbcMetaSqliteLib.Confirmations
{
    public class ConfirmationsTable : IConfirmationsTable
    {
        readonly MetaAppSqliteContext _db_context;
        readonly ILogger<ConfirmationsTable> _logger;
        readonly IOptions<ServerConfigModel> _config;

        public ConfirmationsTable(MetaAppSqliteContext set_db_context, ILogger<ConfirmationsTable> set_logger, IOptions<ServerConfigModel> set_config)
        {
            _db_context = set_db_context;
            _logger = set_logger;
            _config = set_config;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db_context.SaveChangesAsync();
        }

        public async Task AddAsync(ConfirmationModelDb confirmation, bool auto_save = true)
        {
            await _db_context.AddAsync(confirmation);

            if (auto_save)
                await _db_context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ConfirmationModelDb confirmation, bool auto_save = true)
        {
            _db_context.Update(confirmation);

            if (auto_save)
                await _db_context.SaveChangesAsync();
        }

        public async Task<ConfirmationModelDb?> FirstOrDefaultActualAsync(string confirm_id, bool include_user_data = true)
        {
            IQueryable<ConfirmationModelDb> q = _db_context.Confirmations.Where(x => x.ConfirmetAt == null && x.GuidConfirmation == confirm_id && x.Deadline >= DateTime.Now);
#if DEBUG
            var v = q.ToArray();
#endif
            if (include_user_data)
                q = q.Include(x => x.User);

            return await q.FirstOrDefaultAsync();
        }

        public async Task RemoveOutdatedRowsAsync(bool auto_save = true)
        {
#if DEBUG
            var v = _db_context.Confirmations.Where(x => x.Deadline < DateTime.Now.AddDays(-_config.Value.UserManageConfig.ConfirmHistoryDays)).ToArray();
#endif
            _db_context.Confirmations.RemoveRange(_db_context.Confirmations.Where(x => x.Deadline < DateTime.Now.AddDays(-_config.Value.UserManageConfig.ConfirmHistoryDays)));

            if (auto_save)
                await _db_context.SaveChangesAsync();
        }
    }
}
