using MetaLib.ClientServices.refit.users.auth.models;
using MetaLib.Models;
using MetaLib.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Refit;

namespace MetaLib.ClientServices.refit
{
    public class UsersAuthRefitService : IUsersAuthRefitService
    {
        private readonly ILogger<UsersAuthRefitService> _logger;
        private readonly IUsersAuthRefitModel _users_auth_service;
        private readonly IClientSession _session_local_storage;
        private readonly SessionMarkerLiteModel _session_marker;

        public UsersAuthRefitService(ILogger<UsersAuthRefitService> set_logger, SessionMarkerLiteModel set_session_marker, IUsersAuthRefitModel set_users_auth_service, IClientSession set_session_local_storage)
        {
            _logger = set_logger;
            _users_auth_service = set_users_auth_service;
            _session_local_storage = set_session_local_storage;
            _session_marker = set_session_marker;
        }

        public SessionReadResponseRefitModel GetUserSession()
        {
            SessionReadResponseRefitModel result = new SessionReadResponseRefitModel();

            try
            {
                ApiResponse<SessionReadResponseModel>? rest = _users_auth_service.GetUserSession();
                result.StatusCode = rest.StatusCode;
                result.Error = rest.Error;
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    result.IsSuccess = false;

                    result.Message = $"HTTP error: [code={rest.StatusCode}] {rest?.Error?.Content}";
                    _logger.LogError(result.Message);

                    return result;
                }
                result.IsSuccess = true;
                result.SessionMarker = rest.Content.SessionMarker;
                result.Message = rest.Content.Message;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"Exception {nameof(_users_auth_service.GetUserSession)}";
                _logger.LogError(ex, result.Message);

                result.StatusCode = null;
                result.Error = ex;
            }

            return result;
        }

        public async Task<AuthUserResponseRefitModel> LoginUser(UserAuthorizationModel user)
        {
            AuthUserResponseRefitModel result = new AuthUserResponseRefitModel();
            await _session_local_storage.RemoveSessionAsync();
            try
            {
                ApiResponse<AuthUserResponseModel> rest = await _users_auth_service.LoginUser(user);
                result.StatusCode = rest.StatusCode;
                result.Error = rest.Error;
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    result.IsSuccess = false;

                    result.Message = $"HTTP error: [code={rest.StatusCode}] {rest?.Error?.Content}";
                    _logger.LogError(result.Message);

                    return result;
                }
                result.IsSuccess = true;
                result.SessionMarker = rest.Content.SessionMarker;
                result.Message = rest.Content.Message;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"Exception {nameof(_users_auth_service.LoginUser)}";
                _logger.LogError(ex, result.Message);

                result.StatusCode = null;
                result.Error = ex;
            }

            if (result.IsSuccess)
            {
                _session_marker.Reload(result.SessionMarker.Login, result.SessionMarker.AccessLevelUser, result.SessionMarker.Token);
                await _session_local_storage.SaveSessionAsync(_session_marker);
            }

            return result;
        }

        public async Task<ResponseBaseRefitModel> LogOutUser()
        {
            ResponseBaseRefitModel result = new ResponseBaseRefitModel();

            try
            {
                ApiResponse<ResponseBaseModel>? rest = await _users_auth_service.LogOutUser();
                await _session_local_storage.RemoveSessionAsync();
                result.StatusCode = rest.StatusCode;
                result.Error = rest.Error;
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
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

                result.StatusCode = null;
                result.Error = ex;
            }

            return result;
        }

        public async Task<AuthUserResponseRefitModel> RegistrationNewUser(UserRegistrationModel user)
        {
            AuthUserResponseRefitModel result = new AuthUserResponseRefitModel();

            try
            {
                ApiResponse<AuthUserResponseModel> rest = await _users_auth_service.RegistrationNewUser(user);
                await _session_local_storage.RemoveSessionAsync();
                result.StatusCode = rest.StatusCode;
                result.Error = rest.Error;
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    result.IsSuccess = false;

                    result.Message = $"HTTP error: [code={rest.StatusCode}] {rest?.Error?.Content}";
                    _logger.LogError(result.Message);

                    return result;
                }
                result.IsSuccess = true;
                result.SessionMarker = rest.Content.SessionMarker;
                result.Message = rest.Content.Message;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"Exception {nameof(_users_auth_service.RegistrationNewUser)} > {JsonConvert.SerializeObject(user)}";
                _logger.LogError(ex, result.Message);

                result.StatusCode = null;
                result.Error = ex;
            }
            _session_marker.Reload(result.SessionMarker.Login, result.SessionMarker.AccessLevelUser, result.SessionMarker.Token);
            await _session_local_storage.SaveSessionAsync(_session_marker);
            return result;
        }

        public async Task<ResponseBaseRefitModel> RestoreUser(UserRestoreModel user)
        {
            ResponseBaseRefitModel result = new ResponseBaseRefitModel();

            try
            {
                var rest = await _users_auth_service.RestoreUser(user);
                result.StatusCode = rest.StatusCode;
                result.Error = rest.Error;
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
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
                result.Message = $"Exception {nameof(_users_auth_service.RestoreUser)} > {JsonConvert.SerializeObject(user)}";
                _logger.LogError(ex, result.Message);

                result.StatusCode = null;
                result.Error = ex;
            }

            return result;
        }
    }
}
