////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;
using MetaLib.Models;

namespace SrvMetaApp.Repositories
{
    public interface IUsersProfilesRepositoryInterface
    {
        /// <summary>
        /// 
        /// </summary>
        public Task<FindUsersProfilesResponseModel> FindUsersProfilesAsync(FindUsersProfilesRequestModel request);

        public Task<GetUserProfileResponseModel> GetUserProfileAsync(int id);

        public Task<GetUserProfileResponseModel> GetUserProfileAsync(string login);
    }
}
