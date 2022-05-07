////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SharedLib;
using SharedLib.Models;

namespace SrvMetaApp.Repositories
{
    /// <summary>
    /// Сервис работы с подвтерждениями действий пользователя
    /// </summary>
    public class UsersConfirmationsService : IUsersConfirmationsService
    {
        readonly IOptions<ServerConfigModel> _config;
        readonly IMailProviderService _email;
        ILogger<UsersConfirmationsService> _logger;

        readonly IUsersTable _users_dt;
        readonly IConfirmationsTable _confirmations_dt;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="set_logger"></param>
        /// <param name="set_confirmations_dt"></param>
        /// <param name="set_users_dt"></param>
        /// <param name="set_email"></param>
        /// <param name="set_config"></param>
        public UsersConfirmationsService(ILogger<UsersConfirmationsService> set_logger, IConfirmationsTable set_confirmations_dt, IUsersTable set_users_dt, IMailProviderService set_email, IOptions<ServerConfigModel> set_config)
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
        public async Task<ResponseBaseModel?> ConfirmActionAsync(string confirm_id)
        {
            ConfirmationResponseModel? res = await GetConfirmationAsync(confirm_id);

            if (!res.IsSuccess)
            {
                return res;
            }

            switch (res?.Confirmation?.ConfirmationType)
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

        /// <summary>
        /// Получить пдвтерждение пользователя по идентефикатору из БД
        /// </summary>
        /// <param name="confirm_id">Идентификатор подвтерждения пользователя</param>
        /// <param name="include_user_data">Признак необходимости загрузки связанных данных к объекту БД</param>
        /// <returns>Объект подвтерждения действия пользователя (результат запроса)</returns>
        public async Task<ConfirmationResponseModel> GetConfirmationAsync(string confirm_id, bool include_user_data = true)
        {
            ConfirmationResponseModel res = new ConfirmationResponseModel() { IsSuccess = Guid.TryParse(confirm_id, out _) };
            if (!res.IsSuccess)
            {
                res.Message = "Токен подтверждения имеет не корректный формат";
                return res;
            }

            await _confirmations_dt.RemoveOutdatedRowsAsync();

            res.Confirmation = await _confirmations_dt.FirstOrDefaultActualAsync(confirm_id, include_user_data);

            res.IsSuccess = res.Confirmation is not null;
            if (!res.IsSuccess)
            {
                res.Message = "Токен подтверждения не найден. Данный токен просрочен, либо аннулирован";
                return res;
            }

            return res;
        }

        /// <summary>
        /// Создать пдвтерждение пользователя в БД
        /// </summary>
        /// <param name="user">Пользователь, который подтверждает действие</param>
        /// <param name="confirmation_type">Тип подвтерждения действия пользователя</param>
        /// <param name="send_email">Отправить уведомление о создании временной ссылки подвтерждения дейтсвия пользователя</param>
        /// <returns>Объект подвтерждения действия пользователя (результат запроса)</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async Task<ConfirmationResponseModel> CreateConfirmationAsync(UserModelDB user, ConfirmationsTypesEnum confirmation_type, bool send_email = true)
        {
            ConfirmationResponseModel res = new ConfirmationResponseModel() { IsSuccess = true, Message = string.Empty };
            ConfirmationUserActionModelDb confirmation = new ConfirmationUserActionModelDb(user, confirmation_type);
            UserManageConfigModel? user_config = _config.Value.UserManageConfig;

            confirmation.Deadline = confirmation_type switch
            {
                ConfirmationsTypesEnum.RegistrationUser => DateTime.Now.AddMinutes(user_config.RegistrationUserConfirmDeadlineMinutes),
                ConfirmationsTypesEnum.RestoreUser => DateTime.Now.AddMinutes(user_config.RestoreUserConfirmDeadlineMinutes),
                _ => throw new ArgumentOutOfRangeException(nameof(confirmation_type), $"Тип подвтерждения действия '{confirmation_type}' не определён"),
            };

            await _confirmations_dt.AddAsync(confirmation);
            await _confirmations_dt.ReNewAsync(confirmation);

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
