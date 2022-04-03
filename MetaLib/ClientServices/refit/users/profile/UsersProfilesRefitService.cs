////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.ClientServices.refit.users.profile;
using MetaLib.Models;
using MetaLib.Services;
using Microsoft.Extensions.Logging;
using Refit;

namespace MetaLib.ClientServices.refit
{
    public class UsersProfilesRefitService : IUsersProfilesRefitService
    {
        private readonly IUsersProfilesRefitModel _users_profile_service;
        private readonly ILogger<UsersProfilesRefitService> _logger;

        public UsersProfilesRefitService(IUsersProfilesRefitModel set_users_profile_service, ILogger<UsersProfilesRefitService> set_logger)
        {
            _users_profile_service = set_users_profile_service;
            _logger = set_logger;
        }

        public async Task<GetUserProfileResponseRefitModel> GetUserProfileAsync(int client_id)
        {
            GetUserProfileResponseRefitModel result = new GetUserProfileResponseRefitModel();

            try
            {
                ApiResponse<GetUserProfileResponseModel> rest = await _users_profile_service.GetUserProfileAsync(client_id);
                result.StatusCode = rest.StatusCode;
                result.Error = rest.Error;
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    result.IsSuccess = false;

                    result.Message = $"HTTP error: [code={rest.StatusCode}] {rest?.Error?.Content}";
                    _logger.LogError(result.Message);

                    return result;
                }
                result.IsSuccess = true;
                result.User = rest.Content.User;
                result.Message = rest.Content.Message;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"Exception {nameof(_users_profile_service.GetUserProfileAsync)}";
                _logger.LogError(ex, result.Message);

                result.StatusCode = null;
                result.Error = ex;
            }

            return result;
        }

        public async Task<FindUsersProfilesResponseRefitModel> FindUsersProfilesAsync(FindUsersProfilesRequestModel filter)
        {
            FindUsersProfilesResponseRefitModel result = new FindUsersProfilesResponseRefitModel();

            try
            {
                ApiResponse<FindUsersProfilesResponseModel> rest = await _users_profile_service.FindUsersProfilesAsync(filter);
                result.StatusCode = rest.StatusCode;
                result.Error = rest.Error;
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    result.IsSuccess = false;
                    result.Message = $"HTTP error: [code={rest.StatusCode}] {rest?.Error?.Content}";
                    _logger.LogError(result.Message);

                    return result;
                }
                result.IsSuccess = true;
                result = (FindUsersProfilesResponseRefitModel)rest.Content;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"Exception {nameof(_users_profile_service.GetUserProfileAsync)}";
                _logger.LogError(ex, result.Message);

                result.StatusCode = null;
                result.Error = ex;
            }

            return result;
        }
    }
}
