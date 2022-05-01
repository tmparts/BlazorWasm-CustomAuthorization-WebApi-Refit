////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib.Models;
using Refit;

namespace SharedLib.Services
{
    /// <summary>
    /// Регистрация/Авторизация/Вход/Выход пользователя и т.п.
    /// </summary>
    public class UsersAuthRefitProvider : IUsersAuthRefitProvider
    {
        private readonly IUsersAuthRefitService _api;

        public UsersAuthRefitProvider(IUsersAuthRefitService set_api)
        {
            _api = set_api;
        }

        public ApiResponse<SessionReadResponseModel> GetUserSession()
        {
            return _api.GetUserSession();
        }

        public async Task<ApiResponse<AuthUserResponseModel>> LoginUser(UserAuthorizationModel user)
        {
            return await _api.LoginUser(user);
        }

        public async Task<ApiResponse<ResponseBaseModel>> LogOutUser()
        {
            return await _api.LogOutUser();
        }

        public async Task<ApiResponse<AuthUserResponseModel>> RegistrationNewUser(UserRegistrationModel user)
        {
            return await _api.RegistrationNewUser(user);
        }

        public async Task<ApiResponse<ResponseBaseModel>> RestoreUser(UserRestoreModel user)
        {
            return await _api.RestoreUser(user);
        }

#if DEBUG
        public async Task<ApiResponse<WeatherForecastModel[]>> DebugAccessCheck()
        {
            return await _api.DebugAccessCheck();
        }
#endif
    }
}
