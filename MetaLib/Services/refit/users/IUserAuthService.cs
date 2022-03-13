﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;
using Refit;

namespace LibMetaApp.Services
{
    /// <summary>
    /// Регистрация/Авторизация/Вход/Выход пользователя и т.п.
    /// </summary>
    public interface IUserAuthService
    {
        ApiResponse<SessionReadResultModel> GetUserSession();

        Task<ApiResponse<AuthUserResultModel>> RegistrationNewUser(UserRegistrationModel user);

        Task<ApiResponse<AuthUserResultModel>> LoginUser(UserAuthorizationModel user);

        Task<ApiResponse<ResultRequestModel>> LogOutUser();

        Task<ApiResponse<ResultRequestModel>> RestoreUser(UserRestoreModel user);

#if DEBUG
        Task<ApiResponse<WeatherForecastModel[]>> DebugAccessCheck();
#endif
    }
}