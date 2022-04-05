////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using DbcMetaLib.Confirmations;
using DbcMetaLib.Users;
using MetaLib.MemCash;
using MetaLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SrvMetaApp.Models;
using System.Net;

namespace SrvMetaApp.Repositories
{
    public class UsersProfilesRepository : IUsersProfilesRepositoryInterface
    {
        readonly IHttpContextAccessor? _http_context;
        readonly ILogger<UsersProfilesRepository> _logger;
        readonly IOptions<ServerConfigModel> _config;
        readonly IMemoryCashe _mem_cashe;
        readonly IMailServiceInterface _mail;
        readonly ISessionService _session_service;

        IPAddress? RemoteIpAddress => _http_context?.HttpContext?.Request.HttpContext.Connection.RemoteIpAddress;

        readonly IUsersTable _users_dt;
        readonly IConfirmationsTable _confirmations_dt;

        public UsersProfilesRepository(ISessionService set_session_service, ILogger<UsersProfilesRepository> set_logger, IUsersTable set_users_dt, IConfirmationsTable set_confirmations_dt, IMemoryCashe set_mem_cashe, IHttpContextAccessor set_http_context, IMailServiceInterface set_mail, IOptions<ServerConfigModel> set_config) //(, IConfirmationsTable set_confirmations_dt, IUsersConfirmationsInterface set_user_confirmation,  IUsersTable set_users_dt, SessionService set_session_service)
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
                res.IsSuccess = user_db.AccessLevelUser < _session_service.SessionMarker.AccessLevelUser && user.AccessLevelUser <  _session_service.SessionMarker.AccessLevelUser;
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
    }
}
