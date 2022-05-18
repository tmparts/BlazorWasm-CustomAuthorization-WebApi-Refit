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
    /// Сервис работы с сылками пользователей на проекты
    /// </summary>
    public class LinksUsersPrjectsService : ILinksUsersProjectsService
    {
        readonly ILogger<LinksUsersPrjectsService> _logger;
        readonly IOptions<ServerConfigModel> _config;
        readonly IManualMemoryCashe _mem_cashe;
        readonly IMailProviderService _mail;
        readonly ISessionService _session_service;
        readonly ILinksProjectsTable _links_users_to_projects_dt;
        readonly IUsersProjectsTable _projects_dt;

        public LinksUsersPrjectsService(ISessionService set_session_service, ILogger<LinksUsersPrjectsService> set_logger, IUsersProjectsTable set_projects_dt, ILinksProjectsTable set_links_users_to_projects_dt, IManualMemoryCashe set_mem_cashe, IMailProviderService set_mail, IOptions<ServerConfigModel> set_config)
        {
            _logger = set_logger;
            _mem_cashe = set_mem_cashe;
            _config = set_config;
            _mail = set_mail;
            _links_users_to_projects_dt = set_links_users_to_projects_dt;
            _session_service = set_session_service;
            _projects_dt = set_projects_dt;
        }

        public async Task<GetLinksProjectsResponseModel> GetLinksUsersByProject(int project_id)
        {
            GetLinksProjectsResponseModel res = new GetLinksProjectsResponseModel() { IsSuccess = _session_service.SessionMarker.AccessLevelUser > AccessLevelsUsersEnum.Auth };
            if (!res.IsSuccess)
            {
                res.Message = "Вы не подвтердили свою учётную запись.";
                return res;
            }
            ProjectModelDB? project = await _projects_dt.GetProjectAsync(project_id, true);

            res.IsSuccess = project is not null;
            if (!res.IsSuccess)
            {
                res.Message = "Проект не найден.";
                return res;
            }

            res.IsSuccess = !project.IsDeleted || (project.IsDeleted && _session_service.SessionMarker.AccessLevelUser > AccessLevelsUsersEnum.Trusted);
            if (!res.IsSuccess)
            {
                res.Message = "Проект ранее был удалён. Для восстановления обратитесь к администратору.";
                return res;
            }

            res.IsSuccess = project.UsersLinks.Any(x => /*_session_service.SessionMarker.AccessLevelUser >= AccessLevelsUsersEnum.Admin ||*/ ((_session_service.SessionMarker.AccessLevelUser > AccessLevelsUsersEnum.Confirmed || !x.IsDeleted) && (x.UserId == _session_service.SessionMarker.Id)));
            if (!res.IsSuccess)
            {
                res.Message = "Вы не являетесь участником проекта. Обратитесь к автору проекта, что бы он добавил вас в команду.";
                return res;
            }

            res.Links = project.UsersLinks.ToArray();

            return res;
        }
    }
}
