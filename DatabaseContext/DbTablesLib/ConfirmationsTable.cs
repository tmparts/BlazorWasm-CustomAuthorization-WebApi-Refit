////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using DbcLib;
using SharedLib.Models;
using SharedLib.Models.enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SharedLib;

namespace DbcMetaSqliteLib.Confirmations
{
    /// <summary>
    /// Доступ к таблице базы данных: Подтверждение действий пользователя
    /// </summary>
    public class ConfirmationsTable : IConfirmationsTable
    {
        readonly DbAppContext _db_context;
        readonly ILogger<ConfirmationsTable> _logger;
        readonly IOptions<ServerConfigModel> _config;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="set_db_context">Сервис доступа к контексту базы данных</param>
        /// <param name="set_logger">Сервис логирования</param>
        /// <param name="set_config">Конфигурация сервера</param>
        public ConfirmationsTable(DbAppContext set_db_context, ILogger<ConfirmationsTable> set_logger, IOptions<ServerConfigModel> set_config)
        {
            _db_context = set_db_context;
            _logger = set_logger;
            _config = set_config;
        }

        /// <summary>
        /// Сохранить текущие изменения в БД
        /// </summary>
        /// <returns>Количество строк затронутых (или созданных) объектов/строк данных</returns>
        public async Task<int> SaveChangesAsync()
        {
            return await _db_context.SaveChangesAsync();
        }

        /// <summary>
        /// Добавть в таблицу базы данных новое подвтерждение действия
        /// </summary>
        /// <param name="confirmation">Подвтерждение действия</param>
        /// <param name="auto_save">Автоматическое сохранение в БД</param>
        public async Task AddAsync(ConfirmationUserActionModelDb confirmation, bool auto_save = true)
        {
            await _db_context.AddAsync(confirmation);

            if (auto_save)
                await _db_context.SaveChangesAsync();
        }

        /// <summary>
        /// Обновить объект подтверждения действия пользователя
        /// </summary>
        /// <param name="confirmation">Существующее подтверждение действия пользователя</param>
        /// <param name="auto_save">Автоматически сохранять данные в БД</param>
        public async Task UpdateAsync(ConfirmationUserActionModelDb confirmation, bool auto_save = true)
        {
            _db_context.Update(confirmation);

            if (auto_save)
                await _db_context.SaveChangesAsync();
        }

        /// <summary>
        /// Поиск актуальной/непогашеной записи подтверждения действия пользователя
        /// </summary>
        /// <param name="confirm_id">Идентификатор пользователя-владельца подтверждения действия</param>
        /// <param name="include_user_data">Дополнительно загрузхить связанные данные</param>
        /// <returns>Объект подтверждения действия пользователя</returns>
        public async Task<ConfirmationUserActionModelDb?> FirstOrDefaultActualAsync(string confirm_id, bool include_user_data = true)
        {
            IQueryable<ConfirmationUserActionModelDb> q = _db_context.ConfirmationsUsersActions.Where(x => x.ConfirmetAt == null && x.GuidConfirmation == confirm_id && x.Deadline >= DateTime.Now && string.IsNullOrEmpty(x.ErrorMessage));

            if (include_user_data)
                q = q.Include(x => x.User);

            return await q.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Удалить устаревшие записи журнала подтверждений действий пользователей
        /// </summary>
        /// <param name="auto_save">Автоматически сохранять данные в БД</param>
        public async Task RemoveOutdatedRowsAsync(bool auto_save = true)
        {
#if DEBUG
            var v = _db_context.ConfirmationsUsersActions.Where(x => x.Deadline < DateTime.Now.AddDays(-_config.Value.UserManageConfig.ConfirmHistoryDays)).ToArray();
#endif
            _db_context.ConfirmationsUsersActions.RemoveRange(_db_context.ConfirmationsUsersActions.Where(x => x.Deadline < DateTime.Now.AddDays(-_config.Value.UserManageConfig.ConfirmHistoryDays)));

            if (auto_save)
                await _db_context.SaveChangesAsync();
        }

        /// <summary>
        /// Обновить состояние актуальных подтвреждений действий пользователя. Если существуют схожие по смыслу с новым, то они будут деактивированы
        /// </summary>
        /// <param name="confirmation"></param>
        /// <param name="auto_save"></param>
        public async Task ReNewAsync(ConfirmationUserActionModelDb confirmation, bool auto_save = true)
        {
            if ((confirmation?.UserId).GetValueOrDefault(0) == 0 || (confirmation?.Id).GetValueOrDefault(0) == 0)
            {
                return;
            }

            IQueryable<ConfirmationUserActionModelDb>? old_confirmations_query = _db_context.ConfirmationsUsersActions.Where(x => x.Id != confirmation.Id && x.UserId == confirmation.UserId && string.IsNullOrEmpty(x.ErrorMessage) && x.ConfirmetAt == null && x.Deadline >= DateTime.Now);

            if (confirmation.ConfirmationType == ConfirmationsTypesEnum.RestoreUser)
            {
                old_confirmations_query = old_confirmations_query.Where(x => new ConfirmationsTypesEnum[] { ConfirmationsTypesEnum.RestoreUser, ConfirmationsTypesEnum.RegistrationUser }.Contains(x.ConfirmationType));
            }

            List<ConfirmationUserActionModelDb>? old_confirmations = await old_confirmations_query.ToListAsync();
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
