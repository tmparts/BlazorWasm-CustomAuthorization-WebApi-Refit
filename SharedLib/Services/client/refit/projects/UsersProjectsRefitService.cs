////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib.Services;
using Microsoft.Extensions.Logging;
using Refit;
using SharedLib.Models;

namespace SharedLib.ClientServices.refit
{
    public class UsersProjectsRefitService : IUsersProjectsRestService
    {
        private readonly IUsersProjectsRefitService _users_projects_service;
        private readonly ILogger<UsersProjectsRefitService> _logger;

        public UsersProjectsRefitService(IUsersProjectsRefitService set_users_projects_service, ILogger<UsersProjectsRefitService> set_logger)
        {
            _users_projects_service = set_users_projects_service;
            _logger = set_logger;
        }

        public async Task<FindUsersProjectsResponseModel> GetMyProjectsAsync(PaginationRequestModel pagination)
        {
            FindUsersProjectsResponseModel result = new FindUsersProjectsResponseModel();

            try
            {
                ApiResponse<FindUsersProjectsResponseModel> rest = await _users_projects_service.GetMyProjectsAsync(pagination);

                if (rest.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    result.IsSuccess = false;
                    result.Message = $"HTTP error: [code={rest.StatusCode}] {rest?.Error?.Content}";
                    _logger.LogError(result.Message);

                    return result;
                }
                result.IsSuccess = rest.Content.IsSuccess;
                result = rest.Content;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = $"Exception {nameof(_users_projects_service.GetMyProjectsAsync)}";
                _logger.LogError(ex, result.Message);
            }

            return result;
        }
    }
}