////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using CustomPolicyProvider;
using MetaLib.Models;
using Microsoft.AspNetCore.Mvc;
using SrvMetaApp.Models;
using SrvMetaApp.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiMetaApp.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    [MinimumLevelAuthorize(AccessLevelsUsersEnum.Auth)]
    public class UsersProfilesController : ControllerBase
    {
        ISessionService _session_service;
        IUsersProfilesRepositoryInterface _profiles_repo;
        IUsersAuthenticateRepositoryInterface _users_auth_repo;
        ILogger<UsersProfilesController> _logger;

        public UsersProfilesController(IUsersProfilesRepositoryInterface set_profiles_repo, ISessionService set_session_service, IUsersAuthenticateRepositoryInterface set_users_auth_repo, ILogger<UsersProfilesController> set_logger)
        {
            _profiles_repo = set_profiles_repo;
            _users_auth_repo = set_users_auth_repo;
            _logger = set_logger;
            _session_service = set_session_service;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Получить список пользователей")]
        public async Task<FindUsersProfilesResponseModel> Get([FromQuery] FindUsersProfilesRequestModel filter)
        {
            return await _profiles_repo.FindUsersProfilesAsync(filter);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Получить профиль пользователя")]
        [MinimumLevelAuthorize(AccessLevelsUsersEnum.Confirmed)]
        public async Task<GetUserProfileResponseModel> Get([FromRoute] int id)
        {
            if (_session_service.SessionMarker.AccessLevelUser < AccessLevelsUsersEnum.Admin)
                id = 0;

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

        [HttpPut]
        [SwaggerOperation(Summary = "Обновить профиль пользователя")]
        [MinimumLevelAuthorize(AccessLevelsUsersEnum.Confirmed)]
        public async Task<UpdateUserProfileResponseModel> Put([FromBody] UserLiteModel user)
        {
            UpdateUserProfileResponseModel? res = await _profiles_repo.UpdateUserProfileAsync(user);
            return res;
        }
    }
}
