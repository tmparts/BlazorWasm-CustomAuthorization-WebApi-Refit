////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;
using LibMetaApp.Models.enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SrvMetaApp.Repositories
{
    public interface IUsersAuthenticateRepositoryInterface
    {
        /// <summary>
        /// Прочитать текущую сессию пользователя
        /// </summary>
        public SessionReadResultModel ReadMainSession();
        
        /// <summary>
        /// Войти в учётную запись пользователя
        /// </summary>
        public Task<AuthUserResultModel> UserLoginAsync(UserAuthorizationModel user, ModelStateDictionary model_state);
        
        /// <summary>
        /// Выйти из текущей сессии пользователя
        /// </summary>
        public Task<ResultRequestModel> LogOutAsync();
        
        /// <summary>
        /// Найти/прочитать сессию ользователя
        /// </summary>
        public Task<SessionMarkerModel> SessionFind(string guid_token);
        
        /// <summary>
        /// авторизация сессии пользователя (HttpContext)
        /// </summary>
        public Task AuthUserAsync(string login, AccessLevelsUsersEnum access_level_user, int seconds_session);
        
        /// <summary>
        /// Регистрация нового пароля
        /// </summary>
        public Task<AuthUserResultModel> UserRegisterationAsync(UserRegistrationModel new_user, ModelStateDictionary model_state);
        
        /// <summary>
        /// Запрос восстановления доступа к учётной записи
        /// </summary>
        public Task<ResultRequestModel> RestoreUser(UserRestoreModel user);

    }
}
