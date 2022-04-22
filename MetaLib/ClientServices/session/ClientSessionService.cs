////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Blazored.LocalStorage;
using MetaLib.ClientServices.refit;
using MetaLib.Models;
using MetaLib.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Refit;

namespace MetaLib
{
    public class ClientSessionService : IClientSession
    {
        readonly IJSRuntime _js_runtime;
        readonly ILogger<ClientSessionService> _logger;
        readonly SessionMarkerLiteModel _session_marker;
        readonly IUsersAuthRefitModel _users_auth_service;
        readonly ClientConfigModel _config;

        public ClientSessionService(IJSRuntime set_js_runtime, SessionMarkerLiteModel set_session_marker, ClientConfigModel set_config, IUsersAuthRefitModel set_users_auth_service, ILogger<ClientSessionService> set_logger)
        {
            _js_runtime = set_js_runtime;
            _session_marker = set_session_marker;
            _users_auth_service = set_users_auth_service;
            _logger = set_logger;
            _config = set_config;
        }

        public async Task SaveSessionAsync(SessionMarkerLiteModel set_session_marker)
        {
            if (set_session_marker is null)
                return;
            // name, value, seconds, path
            await _js_runtime.InvokeVoidAsync("methods.CreateCookie", GlobalStaticConstants.SESSION_STORAGE_KEY_USER_ID, set_session_marker.Id, _config.CookiesConfig.LongSessionCookieExpiresSeconds, "/");
            await _js_runtime.InvokeVoidAsync("methods.CreateCookie", GlobalStaticConstants.SESSION_STORAGE_KEY_LOGIN, set_session_marker.Login, _config.CookiesConfig.LongSessionCookieExpiresSeconds, "/");
            await _js_runtime.InvokeVoidAsync("methods.CreateCookie", GlobalStaticConstants.SESSION_STORAGE_KEY_LEVEL, set_session_marker.AccessLevelUser, _config.CookiesConfig.LongSessionCookieExpiresSeconds, "/");
            await _js_runtime.InvokeVoidAsync("methods.CreateCookie", GlobalStaticConstants.SESSION_STORAGE_KEY_TOKEN, set_session_marker.Token, _config.CookiesConfig.LongSessionCookieExpiresSeconds, "/");


#if DEBUG
            string? _token = await _js_runtime.InvokeAsync<string>("methods.ReadCookie", GlobalStaticConstants.SESSION_STORAGE_KEY_TOKEN);
            bool read_state_token = !string.IsNullOrEmpty(_token);

            bool read_state_id = int.TryParse(await _js_runtime.InvokeAsync<string>("methods.ReadCookie", GlobalStaticConstants.SESSION_STORAGE_KEY_USER_ID), out int _id);

            string? _login = await _js_runtime.InvokeAsync<string>("methods.ReadCookie", GlobalStaticConstants.SESSION_STORAGE_KEY_LOGIN);
            bool read_state_login = !string.IsNullOrEmpty(_login);

            bool read_state_level = Enum.TryParse(typeof(AccessLevelsUsersEnum), await _js_runtime.InvokeAsync<string>("methods.ReadCookie", GlobalStaticConstants.SESSION_STORAGE_KEY_LEVEL), out object? _level_obj);
            AccessLevelsUsersEnum? _level = read_state_level ? (AccessLevelsUsersEnum?)_level_obj : null;
#endif
        }

        public async Task<SessionMarkerLiteModel> ReadSessionAsync()
        {
            string? _token = await _js_runtime.InvokeAsync<string>("methods.ReadCookie", GlobalStaticConstants.SESSION_STORAGE_KEY_TOKEN);
            bool read_state_token = !string.IsNullOrEmpty(_token);

            bool read_state_id = int.TryParse(await _js_runtime.InvokeAsync<string>("methods.ReadCookie", GlobalStaticConstants.SESSION_STORAGE_KEY_USER_ID), out int _id);

            string? _login = await _js_runtime.InvokeAsync<string>("methods.ReadCookie", GlobalStaticConstants.SESSION_STORAGE_KEY_LOGIN);
            bool read_state_login = !string.IsNullOrEmpty(_login);

            bool read_state_level = Enum.TryParse(typeof(AccessLevelsUsersEnum), await _js_runtime.InvokeAsync<string>("methods.ReadCookie", GlobalStaticConstants.SESSION_STORAGE_KEY_LEVEL), out object? _level_obj);
            AccessLevelsUsersEnum? _level = read_state_level ? (AccessLevelsUsersEnum?)_level_obj : null;

            if (!read_state_token || !read_state_login || !read_state_level || !read_state_id || _id <= 0 || _login.Length < 4 || _level <= AccessLevelsUsersEnum.Anonim)
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
                Id = _id,
                Login = _login,
                Token = _token,
                AccessLevelUser = _level.Value
            };
        }

        public async Task RemoveSessionAsync()
        {
            await _js_runtime.InvokeVoidAsync("methods.DeleteCookie", GlobalStaticConstants.SESSION_STORAGE_KEY_LOGIN);
            await _js_runtime.InvokeVoidAsync("methods.DeleteCookie", GlobalStaticConstants.SESSION_STORAGE_KEY_LEVEL);
            await _js_runtime.InvokeVoidAsync("methods.DeleteCookie", GlobalStaticConstants.SESSION_STORAGE_KEY_TOKEN);
            await _js_runtime.InvokeVoidAsync("methods.DeleteCookie", GlobalStaticConstants.SESSION_STORAGE_KEY_USER_ID);
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
