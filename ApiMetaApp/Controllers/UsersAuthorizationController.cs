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
        IUsersAuthenticateRepositoryInterface _users_repo;

        public UsersAuthorizationController(IUsersAuthenticateRepositoryInterface set_users_repo)
        {
            _users_repo = set_users_repo;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Прочитать информацию о текущей сессии")]
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

        [SwaggerOperation(Summary = "Запрос восстановления доступа к учётной записи")]
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

#if DEBUG

        private Random gen = new Random();

        [SwaggerOperation(Summary = "Проврерка работоспособности")]
        [Authorize]
        [HttpOptions]
        public WeatherForecastModel[] Options()
        {
            return new WeatherForecastModel[]
            {
                RandomDemo(),
                RandomDemo(),
                RandomDemo(),
                RandomDemo(),
                RandomDemo(),
                RandomDemo()
            };
        }

        private WeatherForecastModel RandomDemo()
        {
            WeatherForecastModel res = new WeatherForecastModel();

            res.Date = RandomDay();
            res.TemperatureC = gen.Next(-20, 20);
            if (res.TemperatureC <= -16)
            {
                res.Summary = "Balmy";
            }
            else if (res.TemperatureC <= -13)
            {
                res.Summary = "Freezing";
            }
            else if (res.TemperatureC <= -2)
            {
                res.Summary = "Chilly";
            }
            else if (res.TemperatureC <= 1)
            {
                res.Summary = "Freezing";
            }
            else
            {
                res.Summary = "Bracing";
            }
            return res;
        }

        DateTime RandomDay()
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }
#endif
    }
}
