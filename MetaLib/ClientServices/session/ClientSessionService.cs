////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Blazored.LocalStorage;
using MetaLib.ClientServices.refit;
using MetaLib.Models;
using MetaLib.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Refit;

namespace MetaLib
{
    public class ClientSessionService : IClientSession
    {
        readonly IMemoryCache _memory_cache;
        readonly ILogger<ClientSessionService> _logger;
        readonly SessionMarkerLiteModel _session_marker;
        readonly IUsersAuthRefitModel _users_auth_service;

        public ClientSessionService(IMemoryCache set_memory_cache, SessionMarkerLiteModel set_session_marker, IUsersAuthRefitModel set_users_auth_service, ILogger<ClientSessionService> set_logger)
        {
            _memory_cache = set_memory_cache;
            _session_marker = set_session_marker;
            _users_auth_service = set_users_auth_service;
            _logger = set_logger;
        }

        public async Task SaveSessionAsync(SessionMarkerLiteModel set_session_marker)
        {
            if (set_session_marker is null)
                return;

            _memory_cache.Set(GlobalStaticConstants.SESSION_STORAGE_KEY_USER_ID, set_session_marker.Id);
            _memory_cache.Set(GlobalStaticConstants.SESSION_STORAGE_KEY_LOGIN, set_session_marker.Login);
            _memory_cache.Set(GlobalStaticConstants.SESSION_STORAGE_KEY_LEVEL, set_session_marker.AccessLevelUser);
            _memory_cache.Set(GlobalStaticConstants.SESSION_STORAGE_KEY_TOKEN, set_session_marker.Token);
        }

        public async Task<SessionMarkerLiteModel> ReadSessionAsync()
        {
            bool read_state_token = _memory_cache.TryGetValue(GlobalStaticConstants.SESSION_STORAGE_KEY_TOKEN, out string? _token);
            bool read_state_id = _memory_cache.TryGetValue(GlobalStaticConstants.SESSION_STORAGE_KEY_USER_ID, out int? _id);
            bool read_state_login = _memory_cache.TryGetValue(GlobalStaticConstants.SESSION_STORAGE_KEY_LOGIN, out string? _login);
            bool read_state_level = _memory_cache.TryGetValue(GlobalStaticConstants.SESSION_STORAGE_KEY_LEVEL, out AccessLevelsUsersEnum? _level);

            if (!read_state_token || !read_state_login || !read_state_level || !read_state_id || _id.GetValueOrDefault(0) <= 0 || _login.Length < 4 || _level <= AccessLevelsUsersEnum.Anonim)
            {
                return new SessionMarkerLiteModel()
                {
                    Id = 0,
                    AccessLevelUser = AccessLevelsUsersEnum.Anonim,
                    Login = string.Empty,
                    Token = string.Empty
                };
            }

            return new SessionMarkerLiteModel()
            {
                Id = _id.Value,
                Login = _login,
                Token = _token,
                AccessLevelUser = _level.Value
            };
        }

        public async Task RemoveSessionAsync()
        {
            _memory_cache.Remove(GlobalStaticConstants.SESSION_STORAGE_KEY_LOGIN);
            _memory_cache.Remove(GlobalStaticConstants.SESSION_STORAGE_KEY_LEVEL);
            _memory_cache.Remove(GlobalStaticConstants.SESSION_STORAGE_KEY_TOKEN);
            _memory_cache.Remove(GlobalStaticConstants.SESSION_STORAGE_KEY_USER_ID);
        }

        public async Task<ResponseBaseModel> LogoutAsync()
        {
            ResponseBaseModel? res = new ResponseBaseModel();
            try
            {
                var rest = await _users_auth_service.LogOutUser();

                if (rest.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    _logger.LogError($"HTTP error: [code={rest.StatusCode}]");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error - {nameof(_users_auth_service.LogOutUser)}");
            }

            await RemoveSessionAsync();
            _session_marker.Reload(0, string.Empty, AccessLevelsUsersEnum.Anonim, string.Empty);
            await SaveSessionAsync(_session_marker);

            return res;
        }

        private async Task<ResponseBaseModel> LogOutUser()
        {
            ResponseBaseModel result = new ResponseBaseModel();

            try
            {
                ApiResponse<ResponseBaseModel>? rest = await _users_auth_service.LogOutUser();
                await RemoveSessionAsync();

                if (rest.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    result.IsSuccess = false;

                    result.Message = $"HTTP error: [code={rest.StatusCode}] {rest?.Error?.Content}";
                    _logger.LogError(result.Message);

                    return result;
                }
                result.IsSuccess = true;
                result.Message = rest.Content.Message;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"Exception {nameof(_users_auth_service.LogOutUser)}";
                _logger.LogError(ex, result.Message);
            }

            return result;
        }
    }
}
