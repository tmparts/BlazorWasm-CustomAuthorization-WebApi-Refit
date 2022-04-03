////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.ClientServices.refit.users.profile;
using MetaLib.Models;

namespace MetaLib.ClientServices.refit
{
    public interface IUsersProfilesRefitService
    {
        public Task<GetUserProfileResponseRefitModel> GetUserProfileAsync(int client_id);
        public Task<FindUsersProfilesResponseRefitModel> FindUsersProfilesAsync(FindUsersProfilesRequestModel filter);
    }
}
