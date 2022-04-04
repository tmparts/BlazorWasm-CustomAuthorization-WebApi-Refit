////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models;

namespace MetaLib.ClientServices.refit
{
    public interface IUsersProfilesRefitService
    {
        public Task<GetUserProfileResponseModel> GetUserProfileAsync(int client_id);
        public Task<FindUsersProfilesResponseModel> FindUsersProfilesAsync(FindUsersProfilesRequestModel filter);
    }
}
