﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using DbcMetaLib.Confirmations;
using MetaLib.Models;
using MetaLib.Models.enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SrvMetaApp;

namespace DbcMetaSqliteLib.Confirmations
{
    public class ConfirmationsTable : IConfirmationsTable
    {
        readonly DbAppContext _db_context;
        readonly ILogger<ConfirmationsTable> _logger;
        readonly IOptions<ServerConfigModel> _config;

        public ConfirmationsTable(DbAppContext set_db_context, ILogger<ConfirmationsTable> set_logger, IOptions<ServerConfigModel> set_config)
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
            IQueryable<ConfirmationModelDb> q = _db_context.Confirmations.Where(x => x.ConfirmetAt == null && x.GuidConfirmation == confirm_id && x.Deadline >= DateTime.Now && string.IsNullOrEmpty(x.ErrorMessage));

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

        public async Task ReNewAsync(ConfirmationModelDb confirmation, bool auto_save = true)
        {
            if ((confirmation?.UserId).GetValueOrDefault(0) == 0 || (confirmation?.Id).GetValueOrDefault(0) == 0)
            {
                return;
            }

            IQueryable<ConfirmationModelDb>? old_confirmations_query = _db_context.Confirmations.AsQueryable();

            old_confirmations_query = old_confirmations_query.Where(x => x.Id != confirmation.Id && x.UserId == confirmation.UserId && string.IsNullOrEmpty(x.ErrorMessage) && x.ConfirmetAt == null && x.Deadline >= DateTime.Now);

            if (confirmation.ConfirmationType == ConfirmationsTypesEnum.RestoreUser)
            {
                old_confirmations_query = old_confirmations_query.Where(x => new ConfirmationsTypesEnum[] { ConfirmationsTypesEnum.RestoreUser, ConfirmationsTypesEnum.RegistrationUser }.Contains(x.ConfirmationType));
            }

            List<ConfirmationModelDb>? old_confirmations = await old_confirmations_query.ToListAsync();
            if (old_confirmations.Any())
            {
                old_confirmations.ForEach(x => x.ErrorMessage = $"Создано новое подтверждение: ${JsonConvert.SerializeObject(confirmation)}");
                _db_context.UpdateRange(old_confirmations);
            }
            if (auto_save)
                await _db_context.SaveChangesAsync();
        }
    }
}