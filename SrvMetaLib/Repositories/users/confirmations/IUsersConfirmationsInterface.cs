////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models;
using MetaLib.Models.enums;

namespace SrvMetaApp.Repositories
{
    public interface IUsersConfirmationsInterface
    {
        /// <summary>
        /// Подтвердить действие пользователя
        /// </summary>
        /// <param name="confirm_id">ИД подтверждения</param>
        public Task<ResponseBaseModel> ConfirmActionAsync(string confirm_id);

        public Task<ConfirmationResponseModel> GetConfirmationAsync(string confirm_id, bool include_user_data = true);

        public Task<ConfirmationResponseModel> CreateConfirmationAsync(UserModelDB user, ConfirmationsTypesEnum ConfirmationType, bool send_email = true);        
    }
}
