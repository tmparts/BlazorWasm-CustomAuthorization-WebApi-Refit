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

        public SessionMarkerModel SessionMarker { get; set; } = new SessionMarkerModel(string.Empty, AccessLevelsUsersEnum.Anonim, string.Empty, false);

        public SessionService(IHttpContextAccessor set_http_context, IOptions<ServerConfigModel> set_config, IMemoryCashe set_mem_cashe)
        {
            _httpContext = set_http_context;
            _config = set_config;
            _mem_cashe = set_mem_cashe;
        }

        public async Task InitSession()
        {
            if (_httpContext?.HttpContext?.User.Identity is null)
                return;

            Guid token = ReadTokenFromRequest();
            if (token == Guid.Empty)
            {
                if (_httpContext.HttpContext.User.Identity.IsAuthenticated == true)
                {
                    await _httpContext.HttpContext.SignOutAsync();
                }
                SessionMarker?.Reload(string.Empty, AccessLevelsUsersEnum.Anonim, string.Empty);
                return;
            }
            GuidToken = token.ToString();

            string token_marker = await _mem_cashe.GetStringValueAsync(new MemCasheComplexKeyModel(GuidToken, UsersAuthenticateRepository.PrefRedisSessions));

            if (string.IsNullOrEmpty(token_marker))
            {
                GuidToken = string.Empty;
                if (_httpContext.HttpContext?.User.Identity?.IsAuthenticated == true)
                {
                    await _httpContext.HttpContext.SignOutAsync();
                }
                return;
            }

            SessionMarker = new SessionMarkerModel(token_marker);

            if (string.IsNullOrEmpty(SessionMarker.Login))
            {
                SessionMarker?.Reload(string.Empty, AccessLevelsUsersEnum.Anonim, string.Empty);
                await _httpContext.HttpContext.SignOutAsync();
            }
            else
            {
                SessionMarker.Token = token.ToString();
                if (_httpContext.HttpContext?.User.Identity?.IsAuthenticated != true)
                {
                    await AuthenticateAsync(SessionMarker.Login, SessionMarker.AccessLevelUser.ToString());
                    await _mem_cashe.UpdateValueAsync(UsersAuthenticateRepository.PrefRedisSessions, SessionMarker.Token, SessionMarker.ToString(), TimeSpan.FromSeconds((SessionMarker.IsLongTimeSession ? _config.Value.CookiesConfig.LongSessionCookieExpiresSeconds : _config.Value.CookiesConfig.SessionCookieExpiresSeconds)));
                }
            }
        }

        public Guid ReadTokenFromRequest()
        {
            if (_httpContext.HttpContext.Request.Headers.TryGetValue(GlobalStaticConstants.SESSION_TOKEN_NAME, out StringValues tok))
            {
                string raw_token = tok.FirstOrDefault() ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(raw_token) && Guid.TryParse(raw_token, out Guid parsed_guid))
                {
                    return parsed_guid;
                }
            }

            return Guid.Empty;
        }

        public async Task AuthenticateAsync(string set_login, string set_role)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, set_login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, set_role)
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            ClaimsPrincipal id_claim = new ClaimsPrincipal(id);
            if (_httpContext.HttpContext is not null)
            {
                _httpContext.HttpContext.User = id_claim;
                await _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, id_claim);
                AuthenticateResult? res_auth = await _httpContext.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
    }
}
