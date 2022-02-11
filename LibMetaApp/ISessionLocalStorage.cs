////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;

namespace LibMetaApp
{
    public interface ISessionLocalStorage
    {
        public Task SaveSessionAsync(SessionMarkerLiteModel set_session_marker);
        public Task<SessionMarkerLiteModel> ReadSessionAsync();
        public Task RemoveSessionAsync();

    }
}
