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
        readonly IUsersProjectsTable _users_dt;

        public UsersPrjectsService(ISessionService set_session_service, ILogger<UsersPrjectsService> set_logger, IUsersProjectsTable set_projects_dt, IManualMemoryCashe set_mem_cashe, IMailProviderService set_mail, IOptions<ServerConfigModel> set_config)
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
                    Projects = new ProjectsForUserResponseModel()
                    {
                        RowsData = Array.Empty<ProjectForUserModel>()
                    },
                    IsSuccess = false,
                    Message = ex.Message
                };
            }

            return res;
        }

        /// <summary>
        /// Получить проект
        /// </summary>
        /// <param name="project_id">Идентификатор проекта</param>
        /// <param name="load_links">Загружать ссылки</param>
        /// <returns></returns>
        public async Task<UserProjectResponseModel> GetProjectAsync(int project_id, bool load_links = false)
        {
            UserProjectResponseModel res = new UserProjectResponseModel() { IsSuccess = project_id > 0 };
            if (!res.IsSuccess)
            {
                res.Message = "Идентификатор проекта не может быть <= 0";
                _logger.LogError(res.Message, new ArgumentOutOfRangeException(nameof(project_id)));
                return res;
            }

            try
            {
                res = new UserProjectResponseModel()
                {
                    Project = await _users_dt.GetProjectAsync(project_id, load_links)
                };
                res.IsSuccess = res.Project is not null;
                if (!res.IsSuccess)
                {
                    res.Message = "Проект не найден";
                }
            }
            catch (Exception ex)
            {
                res = new UserProjectResponseModel()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }

            return res;
        }
    }
}
