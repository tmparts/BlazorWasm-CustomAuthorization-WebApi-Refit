////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Blazored.LocalStorage;
using MetaLib.ClientServices.refit;
using MetaLib.Models;
using MetaLib.Services;
using Microsoft.Extensions.Logging;
using Refit;

namespace MetaLib
{
    public class ClientSessionService : IClientSession
    {
        readonly ILocalStorageService _local_store;
        readonly ILogger<ClientSessionService> _logger;
        readonly SessionMarkerLiteModel _session_marker;
        readonly IUsersAuthRefitModel _users_auth_service;

        public ClientSessionService(ILocalStorageService set_local_store, SessionMarkerLiteModel set_session_marker, IUsersAuthRefitModel set_users_auth_service, ILogger<ClientSessionService> set_logger)
        {
            _local_store = set_local_store;
            _session_marker = set_session_marker;
            _users_auth_service = set_users_auth_service;
            _logger = set_logger;
        }

        public async Task SaveSessionAsync(SessionMarkerLiteModel set_session_marker)
        {
            if (set_session_marker is null)
                return;

            await _local_store.SetItemAsStringAsync(GlobalStaticConstants.SESSION_STORAGE_KEY_LOGIN, set_session_marker.Login);
            await _local_store.SetItemAsStringAsync(GlobalStaticConstants.SESSION_STORAGE_KEY_LEVEL, ((int)set_session_marker.AccessLevelUser).ToString());
            await _local_store.SetItemAsStringAsync(GlobalStaticConstants.SESSION_STORAGE_KEY_TOKEN, set_session_marker.Token);
        }

        public async Task<SessionMarkerLiteModel> ReadSessionAsync()
        {
            string? _token = await _local_store.GetItemAsStringAsync(GlobalStaticConstants.SESSION_STORAGE_KEY_TOKEN);
            string? _login = await _local_store.GetItemAsStringAsync(GlobalStaticConstants.SESSION_STORAGE_KEY_LOGIN);
            string? _level_str = await _local_store.GetItemAsStringAsync(GlobalStaticConstants.SESSION_STORAGE_KEY_LEVEL);
            if (!int.TryParse(_level_str, out int _level_int))
            {
                _level_int = (int)AccessLevelsUsersEnum.Anonim;
            }

            return new SessionMarkerLiteModel()
            {
                Login = _login,
                Token = _token,
                AccessLevelUser = (AccessLevelsUsersEnum)_level_int
            };
        }

        public async Task RemoveSessionAsync()
        {
            await _local_store.RemoveItemAsync(GlobalStaticConstants.SESSION_STORAGE_KEY_LOGIN);
            await _local_store.RemoveItemAsync(GlobalStaticConstants.SESSION_STORAGE_KEY_LEVEL);
            await _local_store.RemoveItemAsync(GlobalStaticConstants.SESSION_STORAGE_KEY_TOKEN);
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
                //res.Message = rest?.Message;
                //res.IsSuccess = rest.IsSuccess;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error - {nameof(_users_auth_service.LogOutUser)}");
            }

            await RemoveSessionAsync();
            _session_marker.Reload(string.Empty, AccessLevelsUsersEnum.Anonim, string.Empty);
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
