////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;

namespace LibMetaApp
{
    /// <summary>
    /// Хранение сессии пользователя в Storage браузера
    /// </summary>
    public interface IClientSession
    {
        public Task SaveSessionAsync(SessionMarkerLiteModel set_session_marker);
        public Task<SessionMarkerLiteModel> ReadSessionAsync();
        public Task RemoveSessionAsync();
        public Task<ResponseBaseModel> LogoutAsync();
    }
}
