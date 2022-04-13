////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib;
using MetaLib.MemCash;
using MetaLib.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using SrvMetaApp.Repositories;
using System.Security.Claims;

namespace SrvMetaApp.Models
{
    public class SessionService : SessionServiceLiteModel, ISessionService
    {
        readonly IOptions<ServerConfigModel> _config;
        readonly IHttpContextAccessor _httpContext;
        readonly IMemoryCashe _mem_cashe;

        public SessionMarkerModel SessionMarker { get; set; } = new SessionMarkerModel(0, string.Empty, AccessLevelsUsersEnum.Anonim, string.Empty, false);

        public SessionService(IHttpContextAccessor set_http_context, IOptions<ServerConfigModel> set_config, IMemoryCashe set_mem_cashe)
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

            string token_marker = await _mem_cashe.GetStringValueAsync(new MemCasheComplexKeyModel(GuidToken, UsersAuthenticateRepository.PrefRedisSessions));

            if (string.IsNullOrEmpty(token_marker))
            {
                GuidToken = string.Empty;
                //if (_httpContext.HttpContext?.User.Identity?.IsAuthenticated == true)
                //{
                //    await _httpContext.HttpContext.SignOutAsync();
                //}
                return;
            }

            SessionMarker = new SessionMarkerModel(token_marker);

            if (string.IsNullOrEmpty(SessionMarker.Login))
            {
                SessionMarker?.Reload(0, string.Empty, AccessLevelsUsersEnum.Anonim, string.Empty);
                //await _httpContext.HttpContext.SignOutAsync();
            }
            else
            {
                SessionMarker.Token = token.ToString();
                await _mem_cashe.UpdateValueAsync(UsersAuthenticateRepository.PrefRedisSessions, SessionMarker.Token, SessionMarker.ToString(), TimeSpan.FromSeconds((SessionMarker.IsLongTimeSession ? _config.Value.CookiesConfig.LongSessionCookieExpiresSeconds : _config.Value.CookiesConfig.SessionCookieExpiresSeconds)));
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
