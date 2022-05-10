////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib;
using SharedLib.MemCash;
using SharedLib.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SrvMetaApp.Repositories;

namespace ServerLib
{
    /// <summary>
    /// Сервис работы с пользовательскими проектами
    /// </summary>
    public class UsersPrjectsService : IUsersProjectsService
    {
        readonly ILogger<UsersPrjectsService> _logger;
        readonly IOptions<ServerConfigModel> _config;
        readonly IManualMemoryCashe _mem_cashe;
        readonly IMailProviderService _mail;
        readonly ISessionService _session_service;
        readonly IProjectsTable _users_dt;

        public UsersPrjectsService(ISessionService set_session_service, ILogger<UsersPrjectsService> set_logger, IProjectsTable set_projects_dt, IManualMemoryCashe set_mem_cashe, IMailProviderService set_mail, IOptions<ServerConfigModel> set_config)
        {
            _logger = set_logger;
            _mem_cashe = set_mem_cashe;
            _config = set_config;
            _mail = set_mail;
            _users_dt = set_projects_dt;
            _session_service = set_session_service;
        }

        public async Task<FindUsersProjectsResponseModel> GetMyProjectsAsync(PaginationRequestModel pagination)
        {
            FindUsersProjectsResponseModel res;
            try
            {
                res = new FindUsersProjectsResponseModel()
                {
                    Projects = await _users_dt.GetProjectsForUserAsync((_session_service.SessionMarker.Id, _session_service.SessionMarker.AccessLevelUser), pagination),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                res = new FindUsersProjectsResponseModel()
                {
                    Projects = new ProjectForUserResponseModel()
                    {
                        RowsData = Array.Empty<ProjectForUserModel>()
                    },
                    IsSuccess = false,
                    Message = ex.Message
                };
            }

            return res;
        }
    }
}
