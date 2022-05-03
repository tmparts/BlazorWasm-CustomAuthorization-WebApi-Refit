////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib;
using SharedLib.MemCash;
using SharedLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace ServerLib
{
    /// <summary>
    /// Сервис работы с сессиями 
    /// </summary>
    public class SessionService : SessionLiteService, ISessionService
    {
        readonly IOptions<ServerConfigModel> _config;
        readonly IHttpContextAccessor _httpContext;
        readonly IManualMemoryCashe _mem_cashe;

        /// <summary>
        /// Маркер сессии пользователя
        /// </summary>
        public SessionMarkerModel SessionMarker { get; set; } = new SessionMarkerModel(0, string.Empty, AccessLevelsUsersEnum.Anonim, string.Empty, false);

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="set_http_context"></param>
        /// <param name="set_config"></param>
        /// <param name="set_mem_cashe"></param>
        public SessionService(IHttpContextAccessor set_http_context, IOptions<ServerConfigModel> set_config, IManualMemoryCashe set_mem_cashe)
        {
            _httpContext = set_http_context;
            _config = set_config;
            _mem_cashe = set_mem_cashe;
        }

        public async Task InitSession()
        {
            Guid token = ReadTokenFromRequest();
            if (token == Guid.Empty)
            {
                SessionMarker?.Reload(0, string.Empty, AccessLevelsUsersEnum.Anonim, string.Empty);
                return;
            }
            GuidToken = token.ToString();

            string token_marker = await _mem_cashe.GetStringValueAsync(new MemCasheComplexKeyModel(GuidToken, UsersAuthenticateService.PrefRedisSessions));

            if (string.IsNullOrEmpty(token_marker))
            {
                GuidToken = string.Empty;
                
                return;
            }

            SessionMarker = new SessionMarkerModel(token_marker);

            if (string.IsNullOrEmpty(SessionMarker.Login))
            {
                SessionMarker?.Reload(0, string.Empty, AccessLevelsUsersEnum.Anonim, string.Empty);
            }
            else
            {
                SessionMarker.Token = token.ToString();
                await _mem_cashe.UpdateValueAsync(UsersAuthenticateService.PrefRedisSessions, SessionMarker.Token, SessionMarker.ToString(), TimeSpan.FromSeconds((SessionMarker.IsLongTimeSession ? _config.Value.CookiesConfig.LongSessionCookieExpiresSeconds : _config.Value.CookiesConfig.SessionCookieExpiresSeconds)));
            }
        }

        public Guid ReadTokenFromRequest()
        {
            if (_httpContext.HttpContext.Request.Headers.TryGetValue(_config.Value.CookiesConfig.SessionTokenName, out StringValues tok))
            {
                string raw_token = tok.FirstOrDefault() ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(raw_token) && Guid.TryParse(raw_token, out Guid parsed_guid))
                {
                    return parsed_guid;
                }
            }

            return Guid.Empty;
        }
    }
}
