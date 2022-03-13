////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;

namespace LibMetaApp
{
    /// <summary>
    /// Хранение сессии пользователя в Storage браузера
    /// </summary>
    public interface ISessionLocalStorage
    {
        public Task SaveSessionAsync(SessionMarkerLiteModel set_session_marker);
        public Task<SessionMarkerLiteModel> ReadSessionAsync();
        public Task RemoveSessionAsync();

    }
}
