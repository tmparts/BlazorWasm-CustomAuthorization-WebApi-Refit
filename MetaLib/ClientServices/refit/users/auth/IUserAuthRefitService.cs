////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;
using Refit;

namespace LibMetaApp.Services
{
    /// <summary>
    /// Регистрация/Авторизация/Вход/Выход пользователя и т.п.
    /// </summary>
    public interface IUserAuthRefitService
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
