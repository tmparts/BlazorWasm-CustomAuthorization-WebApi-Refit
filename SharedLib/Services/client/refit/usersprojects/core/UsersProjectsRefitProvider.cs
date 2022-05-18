////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Microsoft.Extensions.Logging;
using Refit;
using SharedLib.Models;

namespace SharedLib.Services
{
    public class UsersProjectsRefitProvider : IUsersProjectsRefitProvider
    {
        private readonly IUsersProjectsRefitService _api;
        private readonly ILogger<UsersProjectsRefitProvider> _logger;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="set_api"></param>
        /// <param name="set_logger"></param>
        public UsersProjectsRefitProvider(IUsersProjectsRefitService set_api, ILogger<UsersProjectsRefitProvider> set_logger)
        {
            _api = set_api;
            _logger = set_logger;
        }

        public async Task<ApiResponse<FindUsersProjectsResponseModel>> GetMyProjectsAsync(PaginationRequestModel pagination)
        {
            return await _api.GetMyProjectsAsync(pagination);
        }

        public async Task<ApiResponse<UserProjectResponseModel>> GetProjectAsync(int id)
        {
            return await _api.GetProjectAsync(id);
        }
    }
}
