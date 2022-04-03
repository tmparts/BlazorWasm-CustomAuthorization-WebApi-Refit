////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models;
using Refit;

namespace MetaLib.Services
{
    /// <summary>
    /// Регистрация/Авторизация/Вход/Выход пользователя и т.п.
    /// </summary>
    public interface IUsersAuthRefitEngine
    {
        ApiResponse<SessionReadResponseModel> GetUserSession();

        Task<ApiResponse<AuthUserResponseModel>> RegistrationNewUser(UserRegistrationModel user);

        Task<ApiResponse<AuthUserResponseModel>> LoginUser(UserAuthorizationModel user);

        Task<ApiResponse<ResponseBaseModel>> LogOutUser();

        Task<ApiResponse<ResponseBaseModel>> RestoreUser(UserRestoreModel user);

#if DEBUG
        Task<ApiResponse<WeatherForecastModel[]>> DebugAccessCheck();
#endif
    }
}
