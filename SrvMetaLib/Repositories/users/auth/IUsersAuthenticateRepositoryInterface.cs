////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SrvMetaApp.Repositories
{
    public interface IUsersAuthenticateRepositoryInterface
    {
        /// <summary>
        /// Прочитать текущую сессию пользователя
        /// </summary>
        public SessionReadResponseModel ReadMainSession();

        /// <summary>
        /// Войти в учётную запись пользователя
        /// </summary>
        public Task<AuthUserResponseModel> UserLoginAsync(UserAuthorizationModel user, ModelStateDictionary model_state);

        /// <summary>
        /// Выйти из текущей сессии пользователя
        /// </summary>
        public Task<ResponseBaseModel> LogOutAsync();

        /// <summary>
        /// Найти/прочитать сессию ользователя
        /// </summary>
        public Task<SessionMarkerModel> SessionFind(string guid_token);

        /// <summary>
        /// авторизация сессии пользователя (HttpContext)
        /// </summary>
        public Task AuthUserAsync(int id, string login, AccessLevelsUsersEnum access_level_user, int seconds_session);

        /// <summary>
        /// Регистрация нового пароля
        /// </summary>
        public Task<AuthUserResponseModel> UserRegisterationAsync(UserRegistrationModel new_user, ModelStateDictionary model_state);

        /// <summary>
        /// Запрос восстановления доступа к учётной записи
        /// </summary>
        public Task<ResponseBaseModel> RestoreUser(UserRestoreModel user);

        /// <summary>
        /// Запрос восстановления доступа к учётной записи
        /// </summary>
        public Task<ResponseBaseModel> RestoreUser(string user_login);
    }
}
