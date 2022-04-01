////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models;

namespace SrvMetaApp.Models
{
    public interface ISessionService
    {
        public SessionMarkerModel SessionMarker { get; set; }
        public string GuidToken { get; set; }

        public Task InitSession();
        public Guid ReadTokenFromRequest();
        public Task AuthenticateAsync(string set_login, string set_role);
    }
}
