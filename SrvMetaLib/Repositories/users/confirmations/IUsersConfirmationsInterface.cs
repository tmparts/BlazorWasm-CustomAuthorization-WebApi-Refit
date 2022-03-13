////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;

namespace SrvMetaApp.Repositories
{
    public interface IUsersConfirmationsInterface
    {
        /// <summary>
        /// Подтвердить действие пользователя
        /// </summary>
        /// <param name="confirm_id">ИД подтверждения</param>
        public Task<ResultRequestModel> ConfirmUserAction(string confirm_id);

        public Task<ConfirmationRequestResultModel> GetConfirmation(string confirm_id, bool include_user_data = true);
    }
}
