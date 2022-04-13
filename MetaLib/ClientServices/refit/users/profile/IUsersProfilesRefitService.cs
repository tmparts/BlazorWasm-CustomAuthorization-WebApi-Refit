////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models;
using MetaLib.Models.api.request;
using MetaLib.Models.enums;

namespace MetaLib.ClientServices.refit
{
    public interface IUsersProfilesRefitService
    {
        public Task<GetUserProfileResponseModel> GetUserProfileAsync(int client_id);

        public Task<FindUsersProfilesResponseModel> FindUsersProfilesAsync(FindUsersProfilesRequestModel filter);

        public Task<UpdateUserProfileResponseModel> UpdateUserProfileAsync(UserLiteModel user);

        public Task<UpdateUserProfileResponseModel> ChangeUserProfileAsync(UserProfileAreasEnum area, ChangeUserProfileOptionsModel user_options);
    }
}
