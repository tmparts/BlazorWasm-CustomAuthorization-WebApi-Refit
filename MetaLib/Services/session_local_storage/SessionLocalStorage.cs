////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Blazored.LocalStorage;
using LibMetaApp.Models;

namespace LibMetaApp
{
    public class SessionLocalStorage : ISessionLocalStorage
    {
        ILocalStorageService _local_store;
        const string SESSION_STORAGE_KEY_LOGIN = "session_login";
        const string SESSION_STORAGE_KEY_LEVEL = "session_level";
        const string SESSION_STORAGE_KEY_TOKEN = "session_token";

        public SessionLocalStorage(ILocalStorageService set_local_store)
        {
            _local_store = set_local_store;
        }

        public async Task SaveSessionAsync(SessionMarkerLiteModel set_session_marker)
        {
            if (set_session_marker is null)
                return;

            await _local_store.SetItemAsStringAsync(SESSION_STORAGE_KEY_LOGIN, set_session_marker.Login);
            await _local_store.SetItemAsStringAsync(SESSION_STORAGE_KEY_LEVEL, ((int)set_session_marker.AccessLevelUser).ToString());
            await _local_store.SetItemAsStringAsync(SESSION_STORAGE_KEY_TOKEN, set_session_marker.Token);
        }

        public async Task<SessionMarkerLiteModel> ReadSessionAsync()
        {
            string? _token = await _local_store.GetItemAsStringAsync(SESSION_STORAGE_KEY_TOKEN);
            string? _login = await _local_store.GetItemAsStringAsync(SESSION_STORAGE_KEY_LOGIN);
            string? _level_str = await _local_store.GetItemAsStringAsync(SESSION_STORAGE_KEY_LEVEL);
            if (!int.TryParse(_level_str, out int _level_int))
            {
                _level_int = (int)AccessLevelsUsersEnum.Anonim;
            }

            return new SessionMarkerLiteModel()
            {
                Login = _login,
                Token = _token,
                AccessLevelUser = (AccessLevelsUsersEnum)_level_int
            };
        }

        public async Task RemoveSessionAsync()
        {
            await _local_store.RemoveItemAsync(SESSION_STORAGE_KEY_LOGIN);
            await _local_store.RemoveItemAsync(SESSION_STORAGE_KEY_LEVEL);
            await _local_store.RemoveItemAsync(SESSION_STORAGE_KEY_TOKEN);
        }
    }
}
