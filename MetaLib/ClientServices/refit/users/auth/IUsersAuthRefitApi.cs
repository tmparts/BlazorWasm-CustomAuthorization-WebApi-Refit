////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;
using Refit;

namespace LibMetaApp.Services
{
    /// <summary>
    /// Refit коннектор к API/UsersAuthorization
    /// </summary>
    [Headers("Content-Type: application/json")]
    public interface IUsersAuthRefitApi
    {
        [Get("/api/UsersAuthorization")]
        ApiResponse<SessionReadResponseModel> GetUserSession();

        [Post("/api/UsersAuthorization")]
        Task<ApiResponse<AuthUserResponseModel>> RegistrationNewUser(UserRegistrationModel user);

        [Put("/api/UsersAuthorization")]
        Task<ApiResponse<AuthUserResponseModel>> LoginUser(UserAuthorizationModel user);

        [Delete("/api/UsersAuthorization")]
        Task<ApiResponse<ResponseBaseModel>> LogOutUser();


        [Patch("/api/UsersAuthorization")]
        Task<ApiResponse<ResponseBaseModel>> RestoreUser(UserRestoreModel user);

#if DEBUG
        [Options("/api/UsersAuthorization")]
        Task<ApiResponse<WeatherForecastModel[]>> DebugAccessCheck();
#endif
    }
}
