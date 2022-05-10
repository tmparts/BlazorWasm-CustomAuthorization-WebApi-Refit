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
    /// Проекты пользователя
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [TypeFilter(typeof(AuthFilterAttributeAsync), Arguments = new object[] { AccessLevelsUsersEnum.Auth })]
    public class UsersProjectsController : ControllerBase
    {
        ISessionService _session_service;
        ILogger<UsersProjectsController> _logger;
        IUsersProjectsService _users_projects_service;

        public UsersProjectsController(ISessionService set_session_service, ILogger<UsersProjectsController> set_logger, IUsersProjectsService set_users_projects_service)
        {
            _logger = set_logger;
            _session_service = set_session_service;
            _users_projects_service = set_users_projects_service;
        }

        [HttpGet]
        public async Task<FindUsersProjectsResponseModel> Get([FromQuery] PaginationRequestModel filter)
        {
            return await _users_projects_service.GetMyProjectsAsync(filter);
        }
    }
}
