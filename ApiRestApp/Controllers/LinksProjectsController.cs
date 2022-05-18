////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using ApiRestApp.Filters;
using Microsoft.AspNetCore.Mvc;
using ServerLib;
using SharedLib.Models;

namespace ApiRestApp.Controllers
{
    /// <summary>
    /// Работа с ссылками пользователей на проекты
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(AuthFilterAttributeAsync), Arguments = new object[] { AccessLevelsUsersEnum.Confirmed })]
    public class LinksProjectsController : ControllerBase
    {
        ILinksUsersProjectsService _links_users_projects_service;

        public LinksProjectsController(ILinksUsersProjectsService set_links_users_projects_service)
        {
            _links_users_projects_service = set_links_users_projects_service;
        }

        /// <summary>
        /// Получить ссылки на проект
        /// </summary>
        /// <param name="project_id">Идентификатор проекта</param>
        /// <returns>Ссылки пользователей на проект</returns>
        [HttpGet]
        public async Task<GetLinksProjectsResponseModel> Get(int project_id)
        {
            return await _links_users_projects_service.GetLinksUsersByProject(project_id);
        }
    }
}
