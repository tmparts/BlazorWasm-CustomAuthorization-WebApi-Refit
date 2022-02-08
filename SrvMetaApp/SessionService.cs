////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp;
using LibMetaApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using SrvMetaApp.Repositories;
using System.Security.Claims;

namespace SrvMetaApp.Models
{
    public class SessionService : SessionServiceLiteModel
    {
        readonly IOptions<ServerConfigModel> _config;
        readonly IHttpContextAccessor _httpContext;
        readonly RedisUtil _redis;

        public SessionMarkerModel? SessionMarker { get; set; } = new SessionMarkerModel(string.Empty, AccessLevelsUsersEnum.Anonim, string.Empty, false);

        public SessionService(IHttpContextAccessor set_http_context, IOptions<ServerConfigModel> set_config, RedisUtil set_redis)
        {
            _httpContext = set_http_context;
            _config = set_config;
            _redis = set_redis;
        }

        public async void InitSession()
        {
            Guid token = ReadTokenFromRequest();
            if (token == Guid.Empty)
            {
                if (_httpContext.HttpContext?.User?.Identity?.IsAuthenticated == true)
                {
                    await _httpContext?.HttpContext?.SignOutAsync();
                }
                return;
            }
            GuidToken = token.ToString();

            string token_marker = await _redis.ValueAsync(new RedisCompKeyExternModel(GuidToken, UsersRepository.PrefRedisSessions));

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
                await _httpContext.HttpContext.SignOutAsync();
            }
            else
            {
                if (_httpContext.HttpContext?.User.Identity?.IsAuthenticated != true)
                {
                    await AuthenticateAsync(SessionMarker.Login, SessionMarker.AccessLevelUser.ToString()/*, SessionMarker.IsLongTimeSession? _config.Value.CookiesConfig.LongSessionCookieExpiresSeconds : _config.Value.CookiesConfig.SessionCookieExpiresSeconds*/);
                    await _redis.UpdateKeyAsync(new KeyValuePair<string, string>(SessionMarker.Token, SessionMarker.ToString()), UsersRepository.PrefRedisSessions, TimeSpan.FromSeconds((SessionMarker.IsLongTimeSession ? _config.Value.CookiesConfig.LongSessionCookieExpiresSeconds : _config.Value.CookiesConfig.SessionCookieExpiresSeconds)));
                }
            }
        }

        public Guid ReadTokenFromRequest()
        {
            if (_httpContext.HttpContext.Request.Headers.TryGetValue(SessionTokenName, out StringValues tok))
            {
                string raw_token = tok.FirstOrDefault() ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(raw_token) && Guid.TryParse(raw_token, out Guid parsed_guid))
                {
                    return parsed_guid;
                }
            }

            return Guid.Empty;
        }

        public bool TokenFromRequestIsLongTime()
        {
            if (_httpContext.HttpContext.Request.Headers.TryGetValue(SessionLongTimeName, out StringValues tok))
            {
                string raw_is_long_time = tok.FirstOrDefault() ?? "false";

                if (!string.IsNullOrWhiteSpace(raw_is_long_time) && bool.TryParse(raw_is_long_time, out bool parsed_guid2))
                {
                    return parsed_guid2;
                }
            }

            return false;
        }

        public async Task AuthenticateAsync(string set_login, string set_role/*, int seconds_session*/)
        {
            // создаем один claim
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, set_login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, set_role)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            ClaimsPrincipal id_claim = new ClaimsPrincipal(id);
            if (_httpContext.HttpContext is not null)
            {
                _httpContext.HttpContext.User = id_claim;
                // установка аутентификационных куки
                await _httpContext.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, id_claim);
                AuthenticateResult? res_auth = await _httpContext.HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }
    }
}
