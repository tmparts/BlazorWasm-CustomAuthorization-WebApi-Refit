////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib;
using SharedLib.MemCash;
using SharedLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;
using SrvMetaApp.Repositories;

namespace ServerLib
{
    /// <summary>
    /// Сервис работы с профилями пользователей
    /// </summary>
    public class UsersProfilesService : IUsersProfilesService
    {
        readonly IHttpContextAccessor? _http_context;
        readonly ILogger<UsersProfilesService> _logger;
        readonly IOptions<ServerConfigModel> _config;
        readonly IManualMemoryCashe _mem_cashe;
        readonly IMailProviderService _mail;
        readonly ISessionService _session_service;
        readonly IUsersTable _users_dt;
        readonly IConfirmationsTable _confirmations_dt;


        IPAddress? RemoteIpAddress => _http_context?.HttpContext?.Request.HttpContext.Connection.RemoteIpAddress;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="set_session_service"></param>
        /// <param name="set_logger"></param>
        /// <param name="set_users_dt"></param>
        /// <param name="set_confirmations_dt"></param>
        /// <param name="set_mem_cashe"></param>
        /// <param name="set_http_context"></param>
        /// <param name="set_mail"></param>
        /// <param name="set_config"></param>
        public UsersProfilesService(ISessionService set_session_service, ILogger<UsersProfilesService> set_logger, IUsersTable set_users_dt, IConfirmationsTable set_confirmations_dt, IManualMemoryCashe set_mem_cashe, IHttpContextAccessor set_http_context, IMailProviderService set_mail, IOptions<ServerConfigModel> set_config) //(, IConfirmationsTable set_confirmations_dt, IUsersConfirmationsInterface set_user_confirmation,  IUsersTable set_users_dt, SessionService set_session_service)
        {
            _logger = set_logger;
            _mem_cashe = set_mem_cashe;
            _http_context = set_http_context;
            _config = set_config;
            _mail = set_mail;
            _users_dt = set_users_dt;
            _confirmations_dt = set_confirmations_dt;
            _session_service = set_session_service;
        }

        public async Task<FindUsersProfilesResponseModel> FindUsersProfilesAsync(FindUsersProfilesRequestModel request)
        {
            return await _users_dt.FindUsersProfilesAsync(request);
        }

        public async Task<GetUserProfileResponseModel> GetUserProfileAsync(int id)
        {
            return await _users_dt.GetUserProfileAsync(id);
        }

        public async Task<GetUserProfileResponseModel> GetUserProfileAsync(string login)
        {
            return await _users_dt.GetUserProfileAsync(login);
        }

        public async Task<UpdateUserProfileResponseModel> UpdateUserProfileAsync(UserLiteModel user)
        {
            UpdateUserProfileResponseModel res = new UpdateUserProfileResponseModel()
            {
                IsSuccess = user is not null,
                User = user
            };

            if (!res.IsSuccess)
            {
                res.Message = "Ошибка обработки запроса. User can'not by NULL";
                return res;
            }

            res.IsSuccess = user.Id > 0;
            if (!res.IsSuccess)
            {
                res.IsSuccess = false;
                res.Message = "Ошибка обработки запроса. User id <= 0";
                return res;
            }

            GetUserProfileResponseModel get_user_db = await _users_dt.GetUserProfileAsync(user.Id);

            res.IsSuccess = get_user_db.IsSuccess;
            if (!res.IsSuccess)
            {
                res.Message = get_user_db.Message;
                return res;
            }

            res.IsSuccess = get_user_db.User != user;
            if (!res.IsSuccess)
            {
                res.Message = "Ошибка обработки запроса. Нет допустимых изменений для сохранения.";
                return res;
            }

            UserModelDB? user_db = get_user_db.User as UserModelDB;

            if (_session_service.SessionMarker.AccessLevelUser > AccessLevelsUsersEnum.Admin)
            {
                user_db.AccessLevelUser = user.AccessLevelUser;
            }
            else
            {
                res.IsSuccess = user_db.AccessLevelUser == _session_service.SessionMarker.AccessLevelUser || (user_db.AccessLevelUser < _session_service.SessionMarker.AccessLevelUser && user.AccessLevelUser < _session_service.SessionMarker.AccessLevelUser);
            }

            if (!res.IsSuccess)
            {
                res.Message = "Не достаточно прав для изменения уровня доступа пользователю. Ваш личный уровень доступа ниже редактируемых.";
                return res;
            }

            res.IsSuccess = user_db.AccessLevelUser == user.AccessLevelUser || _session_service.SessionMarker.AccessLevelUser >= AccessLevelsUsersEnum.Admin;
            if (!res.IsSuccess)
            {
                res.Message = "Не достаточно прав для изменения статуса пользователя.";
                return res;
            }
            if (_session_service.SessionMarker.AccessLevelUser >= AccessLevelsUsersEnum.Admin)
            {
                user_db.Email = user.Email;
                user_db.ConfirmationType = user.ConfirmationType;
                user_db.Login = user.Login;
            }
            user_db.Name = user.Name;
            user_db.About = user.About;
            try
            {
                await _users_dt.UpdateAsync(user_db);
                res.Message = "Данные пользователя успешно сохранены";
                res.User = user_db;
            }
            catch (Exception ex)
            {
                res.IsSuccess = false;
                res.Message = ex.Message;
            }

            return res;
        }

        public async Task<ResponseBaseModel> ChangeUserPasswordAsync(ChangeUserProfileOptionsModel user_options)
        {
            PasswordsPairModel debug_instance;
            try
            {
                debug_instance = JsonConvert.DeserializeObject<PasswordsPairModel>(user_options.OptionAttribute);
            }
            catch (Exception ex)
            {
                return new ResponseBaseModel()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }

            ValidationContext? vc = new ValidationContext(debug_instance, serviceProvider: null, items: null);
            ICollection<ValidationResult> results = new List<ValidationResult>();

            ResponseBaseModel res = new ResponseBaseModel() { IsSuccess = user_options.UserId > 0 };
            if (!res.IsSuccess)
            {
                res.Message = $"Ошибка. Идентификатор пользователя не корректный";
                return res;
            }

            res.IsSuccess = Validator.TryValidateObject(debug_instance, vc, results, true);
            if (!res.IsSuccess)
            {
                res.Message = $"Ошибка валидации модели: {string.Join(";", results.Select(x => $"[{x.ErrorMessage}]"))}.";
                return res;
            }

            res.IsSuccess = debug_instance.PasswordNew == debug_instance.PasswordConfirm;
            if (!res.IsSuccess)
            {
                res.Message = "Новый пароль и подтверждение пароля не совпадают";
                return res;
            }

            res.IsSuccess = _session_service.SessionMarker.AccessLevelUser >= AccessLevelsUsersEnum.Admin || (user_options.UserId == _session_service.SessionMarker.Id && _session_service.SessionMarker.AccessLevelUser >= AccessLevelsUsersEnum.Confirmed);
            if (!res.IsSuccess)
            {
                res.Message = "У вас нет доступа к изменению пароля";
                return res;
            }

            res.IsSuccess = await _users_dt.PasswordEqualByUserIdAsync(user_options.UserId, GlobalUtils.CalculateHashString(debug_instance.PasswordCurrent));
            if (!res.IsSuccess)
            {
                res.Message = "Текущий пароль введён не верно";
                return res;
            }

            await _users_dt.PasswordUpdateByUserIdAsync(user_options.UserId, GlobalUtils.CalculateHashString(debug_instance.PasswordNew));
            res.Message = "Пароль успешно обновлён";

            return res;
        }

        public async Task<ResponseBaseModel> KillUserSessionAsync(ChangeUserProfileOptionsModel user_options)
        {
            throw new NotImplementedException();
        }

        public async Task<UserSessionsPaginationResponseModel> GetUserSessions(int user_id, PaginationRequestModel query)
        {
            throw new NotImplementedException();
        }
    }
}
