////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;
using MetaLib.Models;
using Refit;

namespace LibMetaApp.Services
{
    public class UserProfileRefitService : IUserProfileRefitService
    {
        private readonly IUsersProfileRefitApi _api;

        public UserProfileRefitService(IUsersProfileRefitApi set_api)
        {
            _api = set_api;
        }
        
        public async Task<ApiResponse<FindUsersProfilesResponseModel>> FindUsersProfilesAsync(FindUsersProfilesRequestModel filter)
        {
            return  await _api.FindUsersProfilesAsync(filter);
        }

        public async Task<ApiResponse<GetUserProfileResponseModel>> GetUserProfileAsync(int id)
        {
            return await _api.GetUserProfileAsync(id);
        }
    }
}
