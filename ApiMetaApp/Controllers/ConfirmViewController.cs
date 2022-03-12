////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;
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

        //protected override void Dispose(bool disposing)
        //{
        //    _users_confirmations_repo.Dispose();
        //    base.Dispose(disposing);
        //}

        [HttpGet]
        public async Task<IActionResult> Index(string confirm_id)
        {
            ConfirmationRequestResultModel? res = await _users_confirmations_repo.GetConfirmation(confirm_id);

            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmAction(string confirm_id)
        {
            ResultRequestModel? res = await _users_confirmations_repo.ConfirmUserAction(confirm_id);

            return View(res);
        }
    }
}
