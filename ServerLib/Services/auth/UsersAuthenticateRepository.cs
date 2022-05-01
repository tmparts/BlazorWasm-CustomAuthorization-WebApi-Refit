////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib;
using SharedLib.MemCash;
using SharedLib.Models;
using SharedLib.Models.enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using reCaptcha.Models.VerifyingUsersResponse;
using reCaptcha.stat;
using System.Net;
using SrvMetaApp.Repositories;

namespace ServerLib
{
    /// <summary>
    /// Сервис работы с айтентификацией пользователей
    /// </summary>
    public class UsersAuthenticateRepository : IUsersAuthenticateRepository
    {
        readonly IHttpContextAccessor? _http_context;
        readonly ILogger<UsersAuthenticateRepository> _logger;
        readonly IOptions<ServerConfigModel> _config;
        readonly ISessionService _session_service;
        readonly IManualMemoryCashe _mem_cashe;

        readonly IUsersTable _users_dt;
        readonly IUsersConfirmationsService _confirmations_repo;

        readonly IMailProviderService _mail;

        IPAddress? RemoteIpAddress => _http_context?.HttpContext?.Request.HttpContext.Connection.RemoteIpAddress;

        public static readonly MemCashePrefixModel PrefRedisSessions = new MemCashePrefixModel("sessions", string.Empty);

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="set_logger"></param>
        /// <param name="set_confirmations_repo"></param>
        /// <param name="set_user_confirmation"></param>
        /// <param name="set_mail"></param>
        /// <param name="set_users_dt"></param>
        /// <param name="set_config"></param>
        /// <param name="set_session_service"></param>
        /// <param name="set_mem_cashe"></param>
        /// <param name="set_http_context"></param>
        public UsersAuthenticateRepository(ILogger<UsersAuthenticateRepository> set_logger, IUsersConfirmationsService set_confirmations_repo, IUsersConfirmationsService set_user_confirmation, IMailProviderService set_mail, IUsersTable set_users_dt, IOptions<ServerConfigModel> set_config, ISessionService set_session_service, IManualMemoryCashe set_mem_cashe, IHttpContextAccessor set_http_context)
        {
            _logger = set_logger;
            _session_service = set_session_service;
            _mem_cashe = set_mem_cashe;
            _http_context = set_http_context;
            _config = set_config;
            _users_dt = set_users_dt;
            _confirmations_repo = set_confirmations_repo;
            _mail = set_mail;
        }

        public SessionReadResponseModel ReadMainSession()
        {
            SessionReadResponseModel? res = new SessionReadResponseModel() { IsSuccess = !string.IsNullOrEmpty(_session_service.SessionMarker.Login) };

            if (res.IsSuccess)
            {
                res.SessionMarker = _session_service.SessionMarker;
            }

            return res;
        }

        public async Task<ResponseBaseModel> LogOutAsync()
        {
            if (_http_context?.HttpContext is null)
            {
                return new ResponseBaseModel() { IsSuccess = false, Message = "HttpContext is null" };
            }

            string token = _session_service.ReadTokenFromRequest().ToString();
            if (!string.IsNullOrEmpty(token) && token != Guid.Empty.ToString())
            {
                await _mem_cashe.RemoveKeyAsync(new MemCasheComplexKeyModel(token, PrefRedisSessions));
                _http_context.HttpContext.Response.Cookies.Delete(_config.Value.CookiesConfig.SessionTokenName);
            }

            return new ResponseBaseModel() { IsSuccess = true, Message = "Выход выполнен" };
        }

        public async Task<SessionMarkerModel> SessionFind(string token)
        {
            if (token == null)
            {
                throw new ArgumentNullException();
            }

            string? session_json_raw = await _mem_cashe.GetStringValueAsync(new MemCasheComplexKeyModel(token, PrefRedisSessions));

            _logger.LogDebug(session_json_raw);

            return (SessionMarkerModel)session_json_raw;
        }

        public async Task<AuthUserResponseModel> UserLoginAsync(UserAuthorizationModel user, ModelStateDictionary model_state)
        {
            await LogOutAsync();
            AuthUserResponseModel res = new AuthUserResponseModel() { Message = string.Empty };

            if (_config.Value.ReCaptchaConfig.Mode > ReCaptchaModesEnum.None)
            {
                res.IsSuccess = !string.IsNullOrWhiteSpace(user.ResponseReCAPTCHA);
                if (!res.IsSuccess)
                {
                    res.Message = "Пройдите проверку reCaptcha";
                    return res;
                }
                (reCaptcha2ResponseModel reCaptcha, string Message) reCaptcha2Response = await CheckReCaptcha(user.ResponseReCAPTCHA);
                res.IsSuccess = reCaptcha2Response.reCaptcha.success;
                if (!res.IsSuccess)
                {
                    res.Message = $"Ошибка проверки reCaptcha! {reCaptcha2Response.Message}";
                }
            }

            if (_config.Value.UserManageConfig.DenyAuthorisation.IsDeny)
            {
                res.Message = _config.Value.UserManageConfig.DenyRegistration.Message ?? "Авторизация не возможна по техническим причинам";
                res.IsSuccess = false;
                return res;
            }

            if (!model_state.IsValid)
            {
                res.IsSuccess = false;
                res.Message = string.Join("; ", model_state.Select(x => $"{x.Key}:[{string.Join(". ", x.Value.Errors.Select(y => $"{y.ErrorMessage}"))}]"));
                return res;
            }

            user.Password = GlobalUtils.CalculateHashString(user.Password);
            UserModelDB? user_db = await _users_dt.FirstOrDefaultByLoginAsync(user.Login);
            if (user_db is null || user_db.PasswordHash != user.Password)
            {
                res.IsSuccess = false;
                res.Message = $"Не правильный 'логин' и/или 'пароль'";
                return res;
            }

            await AuthUserAsync(user_db.Id, user_db.Login, user_db.AccessLevelUser, user.RememberMe ? _config.Value.CookiesConfig.LongSessionCookieExpiresSeconds : _config.Value.CookiesConfig.SessionCookieExpiresSeconds);
            SessionReadResponseModel? current_session = ReadMainSession();

            res.IsSuccess = true;
            res.SessionMarker = current_session.SessionMarker;
            return res;
        }

        public async Task<ResponseBaseModel> RestoreUser(string user_login)
        {
            ResponseBaseModel? res = new ResponseBaseModel()
            {
                IsSuccess = !string.IsNullOrWhiteSpace(user_login)
            };
            if (!res.IsSuccess)
            {
                res.Message = "Ошибка. Логин пользователя не может быть пустым.";
                return res;
            }
            UserModelDB? user_db = await _users_dt.FirstOrDefaultByLoginAsync(user_login);
            res.IsSuccess = user_db is not null;
            if (!res.IsSuccess)
            {
                res.Message = $"Ошибка. Пользователь по логину '{user_login}' не найден в БД.";
                return res;
            }
            res.Message = $"Ваш запрос принят. На Email '{user_db.Email}' отправлено сообщение для сброса пароля.";

            ResponseBaseModel confirm_reset_password;

            confirm_reset_password = await _confirmations_repo.CreateConfirmationAsync(user_db, ConfirmationsTypesEnum.RestoreUser);

            if (!confirm_reset_password.IsSuccess)
            {
                res.Message = $"Ошибка. Произошёл сбой отправки Email.\n{confirm_reset_password.Message}".Trim();
                _logger.LogError($"{res.Message} - user_db_by_login: {JsonConvert.SerializeObject(user_db)}");
                res.IsSuccess = false;
            }

            return res;
        }

        public async Task<ResponseBaseModel> RestoreUser(UserRestoreModel user)
        {
            user.Login = user.Login?.Trim() ?? string.Empty;
            user.Email = user.Email?.Trim() ?? string.Empty;

            ResponseBaseModel? res = new ResponseBaseModel()
            {
                IsSuccess = !string.IsNullOrWhiteSpace(user?.Login) || !string.IsNullOrWhiteSpace(user?.Email)
            };

            if (_config.Value.ReCaptchaConfig.Mode > ReCaptchaModesEnum.None)
            {
                res.IsSuccess = !string.IsNullOrWhiteSpace(user.ResponseReCAPTCHA);
                if (!res.IsSuccess)
                {
                    res.Message = "Пройдите проверку reCaptcha";
                    return res;
                }
                (reCaptcha2ResponseModel reCaptcha, string Message) reCaptcha2Response = await CheckReCaptcha(user.ResponseReCAPTCHA);
                res.IsSuccess = reCaptcha2Response.reCaptcha.success;
                if (!res.IsSuccess)
                {
                    res.Message = $"Ошибка проверки reCaptcha! {reCaptcha2Response.Message}";
                }
            }

            if (!res.IsSuccess)
            {
                res.Message = "Укажите логин или email";
                return res;
            }

            UserModelDB? user_db_by_login = string.IsNullOrEmpty(user?.Login)
                ? null
                : await _users_dt.FirstOrDefaultByLoginAsync(user.Login);

            UserModelDB? user_db_by_email = string.IsNullOrEmpty(user?.Email)
                ? null
                : await _users_dt.FirstOrDefaultByEmailAsync(user.Email);

            res.Message = "Ваш запрос принят. Если пользователь с указанными данными есть в системе, то он получит письмо на Email с инструкцией";

            if (user_db_by_login is null && user_db_by_email is null)
            {
                return res;
            }

            ResponseBaseModel confirm_reset_password;
            if (user_db_by_login is not null)
            {
                confirm_reset_password = await _confirmations_repo.CreateConfirmationAsync(user_db_by_login, ConfirmationsTypesEnum.RestoreUser);
                if (!confirm_reset_password.IsSuccess)
                {
                    res.Message = $"Ошибка. Произошёл сбой отправки Email.\n{confirm_reset_password.Message}".Trim();
                    _logger.LogError($"{res.Message} - user_db_by_login: {JsonConvert.SerializeObject(user_db_by_login)}");
                    res.IsSuccess = false;
                }
            }

            if (user_db_by_email is not null)
            {
                confirm_reset_password = await _confirmations_repo.CreateConfirmationAsync(user_db_by_email, ConfirmationsTypesEnum.RestoreUser);
                if (!confirm_reset_password.IsSuccess)
                {
                    res.Message = $"Ошибка. Произошёл сбой отправки Email.\n{confirm_reset_password.Message}".Trim();
                    _logger.LogError($"{res.Message} - user_db_by_email: {JsonConvert.SerializeObject(user_db_by_email)}");
                    res.IsSuccess = false;
                }
            }

            return res;
        }

        public async Task<AuthUserResponseModel> UserRegisterationAsync(UserRegistrationModel new_user, ModelStateDictionary model_state)
        {
            await LogOutAsync();
            AuthUserResponseModel res = new AuthUserResponseModel() { Message = string.Empty };

            if (_config.Value.ReCaptchaConfig.Mode > ReCaptchaModesEnum.None)
            {
                res.IsSuccess = !string.IsNullOrWhiteSpace(new_user.ResponseReCAPTCHA);
                if (!res.IsSuccess)
                {
                    res.Message = "Пройдите проверку reCaptcha";
                    return res;
                }

                (reCaptcha2ResponseModel reCaptcha, string Message) reCaptcha2Response = await CheckReCaptcha(new_user.ResponseReCAPTCHA);
                res.IsSuccess = reCaptcha2Response.reCaptcha.success;
                if (!res.IsSuccess)
                {
                    res.Message = $"Ошибка проверки reCaptcha! {reCaptcha2Response.Message}";
                }
            }

            if (_config.Value.UserManageConfig.DenyRegistration.IsDeny)
            {
                res.Message = _config.Value.UserManageConfig.DenyRegistration.Message ?? "Регистрация не возможна по техническим причинам";
                res.IsSuccess = false;
                return res;
            }

            if (!model_state.IsValid)
            {
                res.IsSuccess = false;
                res.Message = string.Join("; ", model_state.Select(x => $"{x.Key}:[{string.Join(". ", x.Value.Errors.Select(y => $"{y.ErrorMessage}"))}]"));
                return res;
            }

            if (await _users_dt.AnyByLoginOrEmailAsync(new_user.Login, new_user.Email))
            {
                res.IsSuccess = false;
                res.Message = $"'Логин' и/или 'Email' занят. Если вы ранее регистрировали с таким Email, вы можете восстановить доступ к своей учётной записи.";
                return res;
            }
            UserModelDB user_db = (UserModelDB)new_user;
            await _users_dt.AddAsync(user_db);

            ResponseBaseModel confirm_user_registeration = await _confirmations_repo.CreateConfirmationAsync(user_db, ConfirmationsTypesEnum.RegistrationUser);
            if (confirm_user_registeration.IsSuccess)
            {
                await AuthUserAsync(user_db.Id, user_db.Login, user_db.AccessLevelUser);
                SessionReadResponseModel? current_session = ReadMainSession();
                res.IsSuccess = true;
                res.SessionMarker = current_session.SessionMarker;
            }
            else
            {
                res.IsSuccess = false;
                res.SessionMarker = null;
                res.Message = $"Регистрация прошла успешно, но отправка Email с сылкой для подвтерждения учётной записи завершилась с ошибкой: {confirm_user_registeration.Message}";
            }
             
            return res;
        }

        private async Task<(reCaptcha2ResponseModel reCaptcha, string Message)> CheckReCaptcha(string ResponseReCAPTCHA)
        {
            (reCaptcha2ResponseModel reCaptcha, string Message) res = (new reCaptcha2ResponseModel(), string.Empty);

            switch (_config.Value.ReCaptchaConfig.Mode)
            {
                case ReCaptchaModesEnum.Version2:
                    res.reCaptcha = await reCaptchaVerifier.reCaptcha2SiteVerifyAsync(_config.Value.ReCaptchaConfig.ReCaptchaV2Config.PrivateKey, ResponseReCAPTCHA, RemoteIpAddress.ToString());
                    if (res.reCaptcha is null)
                        res.Message = "Сбой работы reCaptcha: res.reCaptcha is null. Попробуйте ещё раз. Если ошибка будет повторяться - сообщите нам об этом.";
                    else if (!res.reCaptcha.success)
                    {
                        res.Message = string.Join(";", res.reCaptcha?.ErrorСodes ?? Array.Empty<string>());
                    }
                    break;
                case ReCaptchaModesEnum.Version2Invisible:
                    res.reCaptcha = await reCaptchaVerifier.reCaptcha2SiteVerifyAsync(_config.Value.ReCaptchaConfig.ReCaptchaV2InvisibleConfig.PrivateKey, ResponseReCAPTCHA, RemoteIpAddress.ToString());

                    if (!res.reCaptcha.success)
                    {
                        res.Message = string.Join(";", res.reCaptcha?.ErrorСodes ?? Array.Empty<string>());
                    }
                    break;
            }
            return res;
        }

        public async Task AuthUserAsync(int id, string login, AccessLevelsUsersEnum access_level, int seconds_session = 0)
        {
            if (seconds_session <= 0)
            {
                seconds_session = _config.Value.CookiesConfig.SessionCookieExpiresSeconds;
            }
            _session_service.GuidToken = Guid.NewGuid().ToString();
            _session_service.SessionMarker = new SessionMarkerModel(id, login, access_level, _session_service.GuidToken, seconds_session > _config.Value.CookiesConfig.SessionCookieExpiresSeconds);

            await _mem_cashe.UpdateValueAsync(PrefRedisSessions, _session_service.GuidToken, _session_service.SessionMarker.ToString(), TimeSpan.FromSeconds(seconds_session));
            await _mem_cashe.UpdateValueAsync(new MemCashePrefixModel("sessions", login), _session_service.GuidToken, $"{DateTime.Now}|{_http_context.HttpContext.Connection.RemoteIpAddress}", TimeSpan.FromSeconds(seconds_session + 60));
        }
    }
}
