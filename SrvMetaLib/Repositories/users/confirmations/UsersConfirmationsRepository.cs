////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using DbcMetaLib.Confirmations;
using DbcMetaLib.Users;
using MetaLib;
using MetaLib.Models;
using MetaLib.Models.enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SrvMetaApp.Models;

namespace SrvMetaApp.Repositories
{
    public class UsersConfirmationsRepository : IUsersConfirmationsInterface
    {
        readonly IOptions<ServerConfigModel> _config;
        readonly IMailServiceInterface _email;
        ILogger<UsersConfirmationsRepository> _logger;

        readonly IUsersTable _users_dt;
        readonly IConfirmationsTable _confirmations_dt;

        public UsersConfirmationsRepository(ILogger<UsersConfirmationsRepository> set_logger, IConfirmationsTable set_confirmations_dt, IUsersTable set_users_dt, IMailServiceInterface set_email, IOptions<ServerConfigModel> set_config, IHttpContextAccessor set_http_context)
        {
            _logger = set_logger;
            _config = set_config;
            _email = set_email;
            _users_dt = set_users_dt;
            _confirmations_dt = set_confirmations_dt;
        }

        /// <summary>
        /// Подтвердить операцию пользователя
        /// </summary>
        /// <param name="confirm_id">токен подтверждения</param>
        public async Task<ResponseBaseModel> ConfirmActionAsync(string confirm_id)
        {
            ConfirmationResponseModel? res = await GetConfirmationAsync(confirm_id);

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
                    await _users_dt.UpdateAsync(res.Confirmation.User, false);

                    res.IsSuccess = await _users_dt.SaveChangesAsync() > 0;

                    if (res.IsSuccess)
                    {
                        res.Message = "Регистрация подтверждена. Авторизуйтесь заново, что бы изменения применились у вас";
                    }
                    else
                    {
                        res.Message = "Ошибка подтверждения регистрации";
                    }

                    await _email.SendEmailAsync(res.Confirmation.User.Email, $"Подтверждение регистрации '{_config.Value.ClientConfig.Host}'", res.Message, MimeKit.Text.TextFormat.Plain);

                    break;
                case ConfirmationsTypesEnum.RestoreUser:

                    res.Confirmation.ConfirmetAt = DateTime.Now;
                    await _confirmations_dt.UpdateAsync(res.Confirmation, false);

                    string? new_pass = GlobalUtils.CreatePassword(9);
                    res.Confirmation.User.PasswordHash = GlobalUtils.CalculateHashString(new_pass);
                    await _users_dt.UpdateAsync(res.Confirmation.User, false);
                    res.IsSuccess = await _users_dt.SaveChangesAsync() > 0;

                    if (res.Confirmation.User.AccessLevelUser < AccessLevelsUsersEnum.Confirmed)
                    {
                        res.Confirmation.User.AccessLevelUser = AccessLevelsUsersEnum.Confirmed;
                        res.Confirmation.User.ConfirmationType = ConfirmationUsersTypesEnum.Email;
                        await _users_dt.UpdateAsync(res.Confirmation.User);
                    }

                    try
                    {
                        await _email.SendEmailAsync(res.Confirmation.User.Email, $"Новый пароль - {_config.Value.ClientConfig.Host}", $"Вам установлен новый пароль: {new_pass}", MimeKit.Text.TextFormat.Html);
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

        public async Task<ConfirmationResponseModel> GetConfirmationAsync(string confirm_id, bool include_user_data = true)
        {
            ConfirmationResponseModel res = new ConfirmationResponseModel() { IsSuccess = Guid.TryParse(confirm_id, out _) };
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

        public async Task<ConfirmationResponseModel> CreateConfirmationAsync(UserModelDB user, ConfirmationsTypesEnum confirmation_type, bool send_email = true)
        {
            ConfirmationResponseModel res = new ConfirmationResponseModel() { IsSuccess = true, Message = string.Empty };
            ConfirmationModelDb confirmation = new ConfirmationModelDb(user, confirmation_type);
            UserManageConfigModel? user_config = _config.Value.UserManageConfig;

            confirmation.Deadline = confirmation_type switch
            {
                ConfirmationsTypesEnum.RegistrationUser => DateTime.Now.AddMinutes(user_config.RegistrationUserConfirmDeadlineMinutes),
                ConfirmationsTypesEnum.RestoreUser => DateTime.Now.AddMinutes(user_config.RestoreUserConfirmDeadlineMinutes),
                _ => throw new ArgumentOutOfRangeException(nameof(confirmation_type), $"Тип подвтерждения действия '{confirmation_type}' не определён"),
            };
            await _confirmations_dt.AddAsync(confirmation);
            res.Confirmation = confirmation;

            if (send_email && !await _email.SendUserConfirmationEmail(confirmation))
            {
                res.Message = "Системная ошибка. Произошёл сбой отправки Email.";
                _logger.LogError($"{res.Message} - confirmation: { JsonConvert.SerializeObject(confirmation)}");
                confirmation.ErrorMessage = res.Message;
                await _confirmations_dt.UpdateAsync(confirmation);
                res.IsSuccess = false;
            }

            return res;
        }
    }
}
