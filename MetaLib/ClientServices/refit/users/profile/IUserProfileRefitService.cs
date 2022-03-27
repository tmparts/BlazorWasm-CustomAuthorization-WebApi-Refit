////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;
using MetaLib.Models;
using Refit;

namespace LibMetaApp.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserProfileRefitService
    {
        Task<ApiResponse<FindUsersProfilesResponseModel>> FindUsersProfilesAsync(FindUsersProfilesRequestModel filter);

        Task<ApiResponse<GetUserProfileResponseModel>> GetUserProfileAsync(int id);
    }
}
