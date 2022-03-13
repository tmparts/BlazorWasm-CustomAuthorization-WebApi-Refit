////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using DbcMetaLib.Confirmations;
using DbcMetaLib.Users;
using LibMetaApp;
using LibMetaApp.Models;
using LibMetaApp.Models.enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SrvMetaApp.Models;

namespace SrvMetaApp.Repositories
{
    public class UsersConfirmationsRepository : IUsersConfirmationsInterface
    {
        readonly IOptions<ServerConfigModel> _config;
        readonly IMailServiceInterface _mail;
        ILogger<UsersConfirmationsRepository> _logger;

        readonly IUsersDb _users_dt;
        readonly IConfirmationsDb _confirmations_dt;

        public UsersConfirmationsRepository(ILogger<UsersConfirmationsRepository> set_logger, IConfirmationsDb set_confirmations_dt, IUsersDb set_users_dt, IMailServiceInterface set_mail, IOptions<ServerConfigModel> set_config, SessionService set_session_service, RedisUtil set_redisUtil, IHttpContextAccessor set_http_context)
        {
            _logger = set_logger;
            _config = set_config;
            _mail = set_mail;
            _users_dt = set_users_dt;
            _confirmations_dt = set_confirmations_dt;
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
                    await _confirmations_dt.UpdateAsync(res.Confirmation, false);

                    res.Confirmation.User.AccessLevelUser = AccessLevelsUsersEnum.Confirmed;
                    res.Confirmation.User.ConfirmationType = ConfirmationUsersTypesEnum.Email;
                    await _users_dt.UpdateAsync(res.Confirmation.User);

                    res.IsSuccess = await _users_dt.SaveChangesAsync() > 0;

                    if (res.IsSuccess)
                    {
                        res.Message = "Регистрация подтверждена. Авторизуйтесь заново, что бы изменения применились у вас";
                    }
                    else
                    {
                        res.Message = "Ошибка подтверждения регистрации";
                    }
                    break;
                case ConfirmationsTypesEnum.RestoreUser:

                    res.Confirmation.ConfirmetAt = DateTime.Now;
                    await _confirmations_dt.UpdateAsync(res.Confirmation, false);

                    string? new_pass = GlobalUtils.CreatePassword(9);
                    res.Confirmation.User.PasswordHash = GlobalUtils.CalculateHashString(new_pass);
                    await _users_dt.UpdateAsync(res.Confirmation.User);
                    res.IsSuccess = await _users_dt.SaveChangesAsync() > 0;

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

        public async Task<ConfirmationRequestResultModel> GetConfirmation(string confirm_id, bool include_user_data = true)
        {
            ConfirmationRequestResultModel res = new ConfirmationRequestResultModel() { IsSuccess = Guid.TryParse(confirm_id, out _) };
            if (!res.IsSuccess)
            {
                res.Message = "токен подтверждения меет не корректный формат";
                return res;
            }

            await _confirmations_dt.RemoveOutdatedRowsAsync();

            res.Confirmation = await _confirmations_dt.FirstOrDefaultActualAsync(confirm_id, include_user_data);

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
