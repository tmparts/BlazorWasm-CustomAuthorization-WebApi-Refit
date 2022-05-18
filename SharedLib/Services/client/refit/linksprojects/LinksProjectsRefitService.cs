////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib.Services;
using Microsoft.Extensions.Logging;
using Refit;
using SharedLib.Models;

namespace SharedLib.ClientServices.refit
{
    public class LinksProjectsRefitService : ILinksProjectsRestService
    {
        private readonly ILinksProjectsRefitService _users_projects_service;
        private readonly ILogger<LinksProjectsRefitService> _logger;

        public LinksProjectsRefitService(ILinksProjectsRefitService set_users_projects_service, ILogger<LinksProjectsRefitService> set_logger)
        {
            _users_projects_service = set_users_projects_service;
            _logger = set_logger;
        }

        public async Task<GetLinksProjectsResponseModel> GetLinksUsersByProject(int project_id)
        {
            GetLinksProjectsResponseModel result = new GetLinksProjectsResponseModel();

            try
            {
                ApiResponse<GetLinksProjectsResponseModel> rest = await _users_projects_service.GetLinksUsersByProject(project_id);

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
                result.Message = $"Exception {nameof(_users_projects_service.GetLinksUsersByProject)}";
                _logger.LogError(ex, result.Message);
            }

            return result;
        }
    }
}