////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SrvMetaApp.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiMetaApp.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    [Authorize]
    public class UsersProfilesController : ControllerBase
    {
        IUsersProfilesRepositoryInterface _profiles_repo;
        IUsersAuthenticateRepositoryInterface _users_auth_repo;
        ILogger<UsersProfilesController> _logger;

        public UsersProfilesController(IUsersProfilesRepositoryInterface set_profiles_repo, IUsersAuthenticateRepositoryInterface set_users_auth_repo, ILogger<UsersProfilesController> set_logger)
        {
            _profiles_repo = set_profiles_repo;
            _users_auth_repo = set_users_auth_repo;
            _logger = set_logger;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Получить список пользователей")]
        public async Task<FindUsersProfilesResponseModel> Get([FromQuery] FindUsersProfilesRequestModel filter)
        {
            return await _profiles_repo.FindUsersProfilesAsync(filter);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Получить профиль пользователя")]
        public async Task<GetUserProfileResponseModel> Get([FromRoute] int id)
        {
            if (id > 0)
                return await _profiles_repo.GetUserProfileAsync(id);
            else
            {
                GetUserProfileResponseModel? res = await _profiles_repo.GetUserProfileAsync(User.Identity.Name);
                if (!res.IsSuccess)
                {
                    string msg = $"Ошибка поиска текущей сессии по имени '{User.Identity.Name}'.";
                    res.Message += msg;
                    _logger.LogError(msg);
                    ResponseBaseModel? logout = await _users_auth_repo.LogOutAsync();
                    if (!logout.IsSuccess)
                    {
                        msg = $"Ошибка закрытия текущей сессии (Name: '{User.Identity.Name}').";
                        res.Message += msg;
                        _logger.LogError(msg);
                    }
                }
                return res;
            }
        }
    }
}
