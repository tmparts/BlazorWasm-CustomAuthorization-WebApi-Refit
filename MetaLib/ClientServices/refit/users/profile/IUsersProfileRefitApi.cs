////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;
using MetaLib.Models;
using Refit;

namespace LibMetaApp.Services
{
    /// <summary>
    /// Refit коннектор к API/UsersProfile
    /// </summary>
    [Headers("Content-Type: application/json")]
    public interface IUsersProfileRefitApi
    {
        [Get("/api/usersprofiles")]
        Task<ApiResponse<FindUsersProfilesResponseModel>> FindUsersProfilesAsync(FindUsersProfilesRequestModel filter);

        [Get("/api/usersprofiles/{id}")]
        Task<ApiResponse<GetUserProfileResponseModel>> GetUserProfileAsync(int? id);
    }
}
