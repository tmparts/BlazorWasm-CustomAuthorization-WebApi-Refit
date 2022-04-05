////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models;
using Microsoft.AspNetCore.Mvc;
using SrvMetaApp.Repositories;

namespace ApiMetaApp.Controllers
{
    [Route("mvc/[controller]")]
    public class ConfirmViewController : Controller
    {
        IUsersConfirmationsInterface _users_confirmations_repo;
        public ConfirmViewController(IUsersConfirmationsInterface set_confirmations_repo)
        {
            _users_confirmations_repo = set_confirmations_repo;
        }

        /// <summary>
        /// Получить токен подтверждения действия пользователя
        /// </summary>
        /// <param name="confirm_id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(string confirm_id)
        {
            ConfirmationResponseModel? res = await _users_confirmations_repo.GetConfirmationAsync(confirm_id);

            return View(res);
        }

        /// <summary>
        /// Подтверждение дейтсвия пользователя
        /// </summary>
        /// <param name="confirm_id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ConfirmAction(string confirm_id)
        {
            ResponseBaseModel? res = await _users_confirmations_repo.ConfirmActionAsync(confirm_id);

            return View(res);
        }
    }
}
