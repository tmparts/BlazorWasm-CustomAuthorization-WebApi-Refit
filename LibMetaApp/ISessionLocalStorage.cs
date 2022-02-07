////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp.Models;

namespace LibMetaApp
{
    public interface ISessionLocalStorage
    {
        //public void SessionSave(SessionMarkerLiteModel set_session_marker);
        public Task SaveSessionAsync(SessionMarkerLiteModel set_session_marker);
        //        
        //public SessionMarkerLiteModel SessionGet();
        public Task<SessionMarkerLiteModel> ReadSessionAsync();
        //        
        //public void SessionRemove();
        public Task RemoveSessionAsync();

    }
}
