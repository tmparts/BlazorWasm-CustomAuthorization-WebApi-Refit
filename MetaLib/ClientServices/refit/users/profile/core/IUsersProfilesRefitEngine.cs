////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models;
using Refit;

namespace MetaLib.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUsersProfilesRefitEngine
    {
        Task<ApiResponse<FindUsersProfilesResponseModel>> FindUsersProfilesAsync(FindUsersProfilesRequestModel filter);

        Task<ApiResponse<GetUserProfileResponseModel>> GetUserProfileAsync(int id);

        Task<ApiResponse<UpdateUserProfileResponseModel>> UpdateUserProfileAsync(UserLiteModel user);
    }
}
