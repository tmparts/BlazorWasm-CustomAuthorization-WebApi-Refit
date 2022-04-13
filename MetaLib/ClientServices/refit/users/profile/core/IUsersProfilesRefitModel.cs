////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models;
using MetaLib.Models.api.request;
using MetaLib.Models.enums;
using Refit;

namespace MetaLib.Services
{
    /// <summary>
    /// Refit коннектор к API/UsersProfile
    /// </summary>
    [Headers("Content-Type: application/json")]
    public interface IUsersProfilesRefitModel
    {
        [Get("/api/usersprofiles")]
        Task<ApiResponse<FindUsersProfilesResponseModel>> FindUsersProfilesAsync(FindUsersProfilesRequestModel filter);

        [Get("/api/usersprofiles/{id}")]
        Task<ApiResponse<GetUserProfileResponseModel>> GetUserProfileAsync(int? id);

        [Put("/api/usersprofiles/")]
        Task<ApiResponse<UpdateUserProfileResponseModel>> UpdateUserProfileAsync(UserLiteModel user);

        [Put("/api/usersprofiles/{area}")]
        Task<ApiResponse<UpdateUserProfileResponseModel>> ChangeUserProfileAsync(UserProfileAreasEnum area, ChangeUserProfileOptionsModel user_options);
    }
}
