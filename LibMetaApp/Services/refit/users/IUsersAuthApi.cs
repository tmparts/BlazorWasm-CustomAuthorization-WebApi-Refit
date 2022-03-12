////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;
using Refit;

namespace LibMetaApp.Services
{
    [Headers("Content-Type: application/json")]
    public interface IUsersAuthApi
    {
        [Get("/api/UsersAuthorization")]
        ApiResponse<SessionReadResultModel> GetUserSession();

        [Post("/api/UsersAuthorization")]
        Task<ApiResponse<AuthUserResultModel>> RegistrationNewUser(UserRegistrationModel user);

        [Put("/api/UsersAuthorization")]
        Task<ApiResponse<AuthUserResultModel>> LoginUser(UserAuthorizationModel user);

        [Delete("/api/UsersAuthorization")]
        Task<ApiResponse<ResultRequestModel>> LogOutUser();


        [Patch("/api/UsersAuthorization")]
        Task<ApiResponse<ResultRequestModel>> RestoreUser(UserRestoreModel user);

#if DEBUG
        [Options("/api/UsersAuthorization")]
        Task<ApiResponse<WeatherForecastModel[]>> DebugAccessCheck();
#endif
    }
}
