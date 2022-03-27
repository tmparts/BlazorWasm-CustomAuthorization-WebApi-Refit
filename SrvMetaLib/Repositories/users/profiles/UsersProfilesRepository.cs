////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using DbcMetaLib.Confirmations;
using DbcMetaLib.Users;
using LibMetaApp.Models;
using MetaLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

namespace SrvMetaApp.Repositories
{
    public class UsersProfilesRepository : IUsersProfilesRepositoryInterface
    {
        readonly IHttpContextAccessor? _http_context;
        readonly ILogger<UsersProfilesRepository> _logger;
        readonly IOptions<ServerConfigModel> _config;
        readonly RedisUtil _redis;
        readonly IMailServiceInterface _mail;
        IPAddress? RemoteIpAddress => _http_context?.HttpContext?.Request.HttpContext.Connection.RemoteIpAddress;

        readonly IUsersTable _users_dt;
        readonly IConfirmationsTable _confirmations_dt;

        public UsersProfilesRepository(ILogger<UsersProfilesRepository> set_logger, IUsersTable set_users_dt, IConfirmationsTable set_confirmations_dt, RedisUtil set_redisUtil, IHttpContextAccessor set_http_context, IMailServiceInterface set_mail, IOptions<ServerConfigModel> set_config) //(, IConfirmationsTable set_confirmations_dt, IUsersConfirmationsInterface set_user_confirmation,  IUsersTable set_users_dt, SessionService set_session_service)
        {
            _logger = set_logger;
            _redis = set_redisUtil;
            _http_context = set_http_context;
            _config = set_config;
            _mail = set_mail;
            _users_dt = set_users_dt;
            _confirmations_dt = set_confirmations_dt;
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
    }
}
