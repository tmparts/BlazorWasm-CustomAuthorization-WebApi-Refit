////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp;
using LibMetaApp.Models;
using LibMetaApp.Models.enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SrvMetaApp.Models;

namespace SrvMetaApp.Repositories
{
    public class UsersRepository : IUsersRepositoryInterface
    {
        readonly IHttpContextAccessor _http_context;
        readonly ILogger<UsersRepository> _logger;
        readonly IOptions<ServerConfigModel> _config;
        readonly SessionService _session_service;
        readonly RedisUtil _redis;
        readonly MetaAppContextDB _db_context;
        readonly IMailInterface _mail;

        public static readonly RedisPrefixExternModel PrefRedisSessions = new RedisPrefixExternModel("sessions", string.Empty);

        public UsersRepository(ILogger<UsersRepository> set_logger, IMailInterface set_mail, MetaAppContextDB set_db_context, IOptions<ServerConfigModel> set_config, SessionService set_session_service, RedisUtil set_redisUtil, IHttpContextAccessor set_http_context)
        {
            _logger = set_logger;
            _session_service = set_session_service;
            _redis = set_redisUtil;
            _http_context = set_http_context;
            _config = set_config;
            _db_context = set_db_context;
            _mail = set_mail;
        }

        public SessionReadResultModel ReadMainSession()
        {
            SessionReadResultModel? res = new SessionReadResultModel() { IsSuccess = !string.IsNullOrEmpty(_session_service.SessionMarker.Login) };

            if (res.IsSuccess)
            {
                res.SessionMarker = _session_service.SessionMarker;
            }

            return res;
        }

        public async Task<ResultRequestModel> LogOutAsync()
        {
            if (!string.IsNullOrEmpty(_session_service.SessionMarker?.Login))
            {
                await _http_context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            string token = _session_service.ReadTokenFromRequest().ToString();
            if (!string.IsNullOrEmpty(token) && token != Guid.Empty.ToString())
            {
                await _redis.RemoveKeyAsync(new RedisCompKeyExternModel(token, PrefRedisSessions));
            }
            return new ResultRequestModel() { IsSuccess = true, Message = "Выход выполнен" };
        }

        public async Task<SessionMarkerModel> SessionFind(string token)
        {
            if (token == null)
            {
                throw new ArgumentNullException();
            }

            string session_json_raw = await _redis.ValueAsync(new RedisCompKeyExternModel(token, PrefRedisSessions));

            _logger.LogDebug(session_json_raw);

            return (SessionMarkerModel)session_json_raw;
        }

        public async Task<AuthUserResultModel> UserLoginAsync(UserAuthorizationModel user, ModelStateDictionary model_state)
        {
            await LogOutAsync();
            AuthUserResultModel res = new AuthUserResultModel() { Message = string.Empty };

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
            UserModelDB? user_db = await _db_context.Users.FirstOrDefaultAsync(x => x.Login == user.Login);
            if (user_db is null)
            {
                res.IsSuccess = false;
                res.Message = $"Не правильный 'логин' и/или 'пароль'";
                return res;
            }

            await AuthUserAsync(user_db.Login, user_db.AccessLevelUser, user.RememberMe ? _config.Value.CookiesConfig.LongSessionCookieExpiresSeconds : _config.Value.CookiesConfig.SessionCookieExpiresSeconds);
            SessionReadResultModel? current_session = ReadMainSession();

            res.IsSuccess = true;
            res.SessionMarker = current_session.SessionMarker;
            return res;
        }

        public async Task<ResultRequestModel> RestoreUser(UserRestoreModel user)
        {
            ResultRequestModel? res = new ResultRequestModel()
            {
                IsSuccess = !string.IsNullOrWhiteSpace(user?.Login) || !string.IsNullOrWhiteSpace(user?.Email)
            };

            if (!res.IsSuccess)
            {
                res.Message = "Укажите логин или email";
                return res;
            }

            user.Login = user.Login?.Trim() ?? string.Empty;
            user.Email = user.Email?.Trim() ?? string.Empty;

            UserModelDB? user_db_by_login = string.IsNullOrEmpty(user.Login) ? null : await _db_context.Users.FirstOrDefaultAsync(x => x.Login == user.Login);
            UserModelDB? user_db_by_email = string.IsNullOrEmpty(user.Email) ? null : await _db_context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);

            res.Message = "Ваш запрос принят. Если пользователь с указанными данными есть в системе, то он получит письмо на Email с инструкцией";

            if (user_db_by_login is null && user_db_by_email is null)
            {
                return res;
            }

            if (!await _mail.SendEmailRestoreUser(user_db_by_login))
            {
                res.Message = "Системная ошибка. Произошёл сбой отправки Email. ";
                _logger.LogError(res.Message);
            }

            return res;
        }

        public async Task<ResultRequestModel> ConfirmationUserAction(ConfirmationsTypesEnum confirm_type, string confirm_id)
        {
            ResultRequestModel res = new ResultRequestModel() { IsSuccess = Guid.TryParse(confirm_id, out _) };
            if (!res.IsSuccess)
            {
                res.Message = "токен подтверждения меет не корректный формат";
                return res;
            }
            _db_context.Confirmations.RemoveRange(_db_context.Confirmations.Where(x => x.Deadline > DateTime.Now.AddDays(_config.Value.UserManageConfig.ConfirmHistoryDays)));
            await _db_context.SaveChangesAsync();

            ConfirmationModelDb? confirmation_db = await _db_context.Confirmations.Include(x => x.User).FirstOrDefaultAsync(x => x.ConfirmetAt == null && x.Guid == confirm_id && x.ConfirmationType == confirm_type && x.Deadline >= DateTime.Now.AddMinutes(_config.Value.UserManageConfig.RegistrationUserConfirmDeadlineMinutes));
            res.IsSuccess = confirmation_db is null;
            if (!res.IsSuccess)
            {
                res.Message = "токен подтверждения не найден или просрочен";
                return res;
            }

            switch (confirm_type)
            {
                case ConfirmationsTypesEnum.RegistrationUser:

                    confirmation_db.ConfirmetAt = DateTime.Now;
                    _db_context.Update(confirmation_db);

                    confirmation_db.User.AccessLevelUser = AccessLevelsUsersEnum.Confirmed;
                    _db_context.Update(confirmation_db.User);

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

                    confirmation_db.ConfirmetAt = DateTime.Now;
                    _db_context.Update(confirmation_db);

                    string? new_pass = GlobalUtils.CreatePassword(9);
                    confirmation_db.User.PasswordHash = GlobalUtils.CalculateHashString(new_pass);
                    _db_context.Update(confirmation_db.User);
                    res.IsSuccess = await _db_context.SaveChangesAsync() > 0;
                    if (res.IsSuccess)
                    {
                        res.Message = $"Ваш новый пароль: {new_pass}.";
                    }
                    else
                    {
                        res.Message = "Ошибка сброса пароля";
                    }
                    break;
            }

            return res;
        }

        public async Task<AuthUserResultModel> UserRegisterationAsync(UserRegistrationModel new_user, ModelStateDictionary model_state)
        {
            await LogOutAsync();
            AuthUserResultModel res = new AuthUserResultModel() { Message = string.Empty };

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

            if (await _db_context.Users.AnyAsync(x => x.Login == new_user.Login || x.Email == new_user.Email))
            {
                res.IsSuccess = false;
                res.Message = $"'Логин' и/или 'Email' занят.";
                return res;
            }
            UserModelDB user_db = (UserModelDB)new_user;
            await _db_context.Users.AddAsync(user_db);
            await _db_context.SaveChangesAsync();
            ConfirmationModelDb confirm_registration = new ConfirmationModelDb("name", user_db, Guid.NewGuid().ToString(), ConfirmationsTypesEnum.RegistrationUser, DateTime.Now.AddSeconds(0)) { };
            await _mail.SendEmailConfirmUser(user_db, confirm_registration);


            await AuthUserAsync(user_db.Login, user_db.AccessLevelUser);
            SessionReadResultModel? current_session = ReadMainSession();

            res.IsSuccess = true;
            res.SessionMarker = current_session.SessionMarker;
            return res;
        }

        public async Task AuthUserAsync(string login, AccessLevelsUsersEnum access_level, int seconds_session = 0)
        {
            if (seconds_session <= 0)
            {
                seconds_session = _config.Value.CookiesConfig.SessionCookieExpiresSeconds;
            }
            _session_service.GuidToken = Guid.NewGuid().ToString();
            _session_service.SessionMarker = new SessionMarkerModel(login, access_level, _session_service.GuidToken, seconds_session > _config.Value.CookiesConfig.SessionCookieExpiresSeconds);
            await _session_service.AuthenticateAsync(login, access_level.ToString()/*, seconds_session*/);
            await _redis.UpdateKeyAsync(new KeyValuePair<string, string>(_session_service.GuidToken, _session_service.SessionMarker.ToString()), PrefRedisSessions, TimeSpan.FromSeconds(seconds_session));
        }
    }
}
