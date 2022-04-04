////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Refit;

namespace MetaLib.Services
{
    public class UsersProfilesRefitEngine : IUsersProfilesRefitEngine
    {
        private readonly IUsersProfilesRefitModel _api;
        private readonly ILogger<UsersProfilesRefitEngine> _logger;

        public UsersProfilesRefitEngine(IUsersProfilesRefitModel set_api, ILogger<UsersProfilesRefitEngine> set_logger)
        {
            _api = set_api;
            _logger = set_logger;
        }

        public async Task<ApiResponse<FindUsersProfilesResponseModel>> FindUsersProfilesAsync(FindUsersProfilesRequestModel filter)
        {
#if DEBUG
            _logger.LogDebug($"refit request > {nameof(FindUsersProfilesAsync)}({nameof(filter)}: {JsonConvert.SerializeObject(FindUsersProfilesAsync)})");
#endif
            return await _api.FindUsersProfilesAsync(filter);
        }

        public async Task<ApiResponse<GetUserProfileResponseModel>> GetUserProfileAsync(int id)
        {
#if DEBUG
            _logger.LogDebug($"refit request > {nameof(GetUserProfileAsync)}({nameof(id)}: {id})");
#endif
            return await _api.GetUserProfileAsync(id);
        }

        public async Task<ApiResponse<UpdateUserProfileResponseModel>> UpdateUserProfileAsync(UserLiteModel user)
        {
            return await _api.UpdateUserProfileAsync(user);
        }
    }
}
