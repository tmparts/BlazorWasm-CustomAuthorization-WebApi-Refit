////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp;
using LibMetaApp.Models;
using LibMetaApp.Models.enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SrvMetaApp.Models;

namespace SrvMetaApp.Repositories
{
    public class UsersConfirmationsRepository : IUsersConfirmationsInterface
    {
        readonly IHttpContextAccessor _http_context;
        readonly ILogger<UsersConfirmationsRepository> _logger;
        readonly IOptions<ServerConfigModel> _config;
        readonly SessionService _session_service;
        readonly RedisUtil _redis;
        readonly MetaAppContextDB _db_context;
        readonly IMailInterface _mail;

        public UsersConfirmationsRepository(ILogger<UsersConfirmationsRepository> set_logger, IMailInterface set_mail, MetaAppContextDB set_db_context, IOptions<ServerConfigModel> set_config, SessionService set_session_service, RedisUtil set_redisUtil, IHttpContextAccessor set_http_context)
        {
            _logger = set_logger;
            _session_service = set_session_service;
            _redis = set_redisUtil;
            _http_context = set_http_context;
            _config = set_config;
            _db_context = set_db_context;
            _mail = set_mail;
        }

        /// <summary>
        /// Подтвердить операцию пользователя
        /// </summary>
        /// <param name="confirm_id">токен подтверждения</param>
        public async Task<ResultRequestModel> ConfirmUserAction(string confirm_id)
        {
            ConfirmationRequestResultModel? res = await GetConfirmation(confirm_id);

            if (!res.IsSuccess)
            {
                return res;
            }

            switch (res.Confirmation.ConfirmationType)
            {
                case ConfirmationsTypesEnum.RegistrationUser:

                    if (res.Confirmation.User.AccessLevelUser > AccessLevelsUsersEnum.Auth)
                    {
                        res.IsSuccess = false;
                        res.Message = "Пользователь не требует подтверждения";
                        break;
                    }

                    res.Confirmation.ConfirmetAt = DateTime.Now;
                    _db_context.Update(res.Confirmation);

                    res.Confirmation.User.AccessLevelUser = AccessLevelsUsersEnum.Confirmed;
                    res.Confirmation.User.ConfirmationType = ConfirmationUsersTypesEnum.Email;
                    _db_context.Update(res.Confirmation.User);

                    res.IsSuccess = await _db_context.SaveChangesAsync() > 0;

                    if (res.IsSuccess)
                    {
                        res.Message = "Регистрация подтверждена. Авторизуйтесь заново, что бы изменения отразились у вас на клиенте";
                    }
                    else
                    {
                        res.Message = "Ошибка подтверждения регистрации";
                    }
                    break;
                case ConfirmationsTypesEnum.RestoreUser:

                    res.Confirmation.ConfirmetAt = DateTime.Now;
                    _db_context.Update(res.Confirmation);

                    string? new_pass = GlobalUtils.CreatePassword(9);
                    res.Confirmation.User.PasswordHash = GlobalUtils.CalculateHashString(new_pass);
                    _db_context.Update(res.Confirmation.User);
                    res.IsSuccess = await _db_context.SaveChangesAsync() > 0;

                    try
                    {
                        await _mail.SendEmailAsync(res.Confirmation.User.Email, "Новый пароль - IQ-S.pro", $"Вам установлен новый пароль: {new_pass}", MimeKit.Text.TextFormat.Html);
                    }
                    catch
                    {

                    }

                    if (res.IsSuccess)
                    {
                        res.Message = $"Ваш новый пароль: {new_pass}. Он так же отправлен на ваш Email";
                    }
                    else
                    {
                        res.Message = "Ошибка сброса пароля";
                    }
                    break;
            }

            return res;
        }

        public async Task<ConfirmationRequestResultModel> GetConfirmation(string confirm_id)
        {
            ConfirmationRequestResultModel res = new ConfirmationRequestResultModel() { IsSuccess = Guid.TryParse(confirm_id, out _) };
            if (!res.IsSuccess)
            {
                res.Message = "токен подтверждения меет не корректный формат";
                return res;
            }
            _db_context.Confirmations.RemoveRange(_db_context.Confirmations.Where(x => x.Deadline > DateTime.Now.AddDays(_config.Value.UserManageConfig.ConfirmHistoryDays)));
            await _db_context.SaveChangesAsync();

            res.Confirmation = await _db_context.Confirmations.Include(x => x.User).FirstOrDefaultAsync(x => x.ConfirmetAt == null && x.Guid == confirm_id && x.Deadline >= DateTime.Now);

            res.IsSuccess = res.Confirmation is not null;
            if (!res.IsSuccess)
            {
                res.Message = "токен подтверждения не найден или просрочен";
                return res;
            }

            return res;
        }
    }
}
