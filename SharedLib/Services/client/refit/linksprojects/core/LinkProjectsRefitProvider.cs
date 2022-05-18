////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Microsoft.Extensions.Logging;
using Refit;
using SharedLib.Models;

namespace SharedLib.Services
{
    public class LinksProjectsRefitProvider : ILinksProjectsRefitProvider
    {
        private readonly ILinksProjectsRefitService _api;
        private readonly ILogger<LinksProjectsRefitProvider> _logger;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="set_api"></param>
        /// <param name="set_logger"></param>
        public LinksProjectsRefitProvider(ILinksProjectsRefitService set_api, ILogger<LinksProjectsRefitProvider> set_logger)
        {
            _api = set_api;
            _logger = set_logger;
        }
        
        public async Task<ApiResponse<GetLinksProjectsResponseModel>> GetLinksUsersByProject(int project_id)
        {
            return await _api.GetLinksUsersByProject(project_id);
        }
    }
}
