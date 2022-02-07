////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;
using Refit;

namespace LibMetaApp.Services
{
    public class UserAuthService : IUsersService
    {
        private readonly IUsersAuthApi _api;

        public UserAuthService(IUsersAuthApi set_api)
        {
            _api = set_api;
        }

        public ApiResponse<SessionReadResultModel> GetUserSession()
        {
            return _api.GetUserSession();
        }

        public async Task<ApiResponse<AuthUserResultModel>> LoginUser(UserAuthorizationModel user)
        {
            return await _api.LoginUser(user);
        }

        public async Task<ApiResponse<ResultRequestModel>> LogOutUser()
        {
            return await _api.LogOutUser();
        }

        public async Task<ApiResponse<AuthUserResultModel>> RegistrationNewUser(UserRegistrationModel user)
        {
            return await _api.RegistrationNewUser(user);
        }

        public async Task<ApiResponse<ResultRequestModel>> RestoreUser(UserRestoreModel user)
        {
            return await _api.RestoreUser(user);
        }
    }
}
