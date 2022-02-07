////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SrvMetaApp.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace ApiMetaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersAuthorizationController : ControllerBase
    {
        IUsersRepositoryInterface _users_repo;

        public UsersAuthorizationController(IUsersRepositoryInterface set_users_repo)
        {
            _users_repo = set_users_repo;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Прочитать информацию о текущей сессии сессии")]
        public SessionReadResultModel Get([FromQuery] string? ReturnUrl)
        {
            return _users_repo.ReadMainSession();
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Регистрация нового пользователя")]
        public async Task<AuthUserResultModel> Post([FromBody] UserRegistrationModel user)
        {
            return await _users_repo.UserRegisterationAsync(user, ModelState);
        }

        [SwaggerOperation(Summary = "Авторизация пользователя")]
        [HttpPut]
        public async Task<AuthUserResultModel> Put([FromBody] UserAuthorizationModel user)
        {
            return await _users_repo.UserLoginAsync(user, ModelState);
        }

        [SwaggerOperation(Summary = "Восстановление доступа к учётной записи")]
        [HttpPatch]
        public async Task<ResultRequestModel> Patch([FromBody] UserRestoreModel user)
        {
            return await _users_repo.RestoreUser(user);
        }

        [SwaggerOperation(Summary = "Выход из текущей сессии")]
        [Authorize]
        [HttpDelete]
        public async Task<ResultRequestModel> DeleteAsync()
        {
           return await _users_repo.LogOutAsync();
        }
    }
}
