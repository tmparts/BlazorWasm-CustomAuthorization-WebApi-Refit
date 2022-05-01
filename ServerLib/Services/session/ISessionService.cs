////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib.Models;

namespace ServerLib
{
    /// <summary>
    /// Сервис работы с сессиями 
    /// </summary>
    public interface ISessionService
    {
        /// <summary>
        /// Маркер сессии пользователя
        /// </summary>
        public SessionMarkerModel SessionMarker { get; set; }

        /// <summary>
        /// Токен сессии пользователя
        /// </summary>
        public string GuidToken { get; set; }

        /// <summary>
        /// Инициализация сессии (чтение кукисов, заголовков и данных мемкеша)
        /// </summary>
        public Task InitSession();

        /// <summary>
        /// Чтение кукисов и заголовков http запроса и последующая сверка информации с бд сессий
        /// </summary>
        /// <returns>Уникальный токен сессии пользователя</returns>
        public Guid ReadTokenFromRequest();
        //public Task AuthenticateAsync(string set_login, string set_role);
    }
}
