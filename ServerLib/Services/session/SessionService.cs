////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib;
using SharedLib.MemCash;
using SharedLib.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using StackExchange.Redis;
using System;
using System.Net;

namespace ServerLib
{
    /// <summary>
    /// Сервис работы с сессиями 
    /// </summary>
    public class SessionService : SessionLiteService, ISessionService
    {
        readonly IOptions<ServerConfigModel> _config;
        readonly IHttpContextAccessor _http_context;
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
            _http_context = set_http_context;
            _config = set_config;
            _mem_cashe = set_mem_cashe;
        }

        /// <summary>
        /// Получить список текущий/действующих сессий пользователя по логину
        /// </summary>
        /// <param name="login">Логин пользователя, сессии которого нужны</param>
        /// <returns>Список текущий/действующих сессий пользователя</returns>
        public async Task<List<SessionStorageModel>> GetUserSessionsAsync(string login)
        {
            List<SessionStorageModel> res = new List<SessionStorageModel>();

            string user_session_data = null;
            string[] data_segments;

            string user_session_key = null;
            string[] user_session_key_segments;

            List<RedisKey>? sessions = _mem_cashe.FindKeys(new MemCashePrefixModel("sessions", $"{login}*"));
            foreach (RedisKey key_session in sessions)
            {
                user_session_key = key_session.ToString();
                user_session_key_segments = user_session_key.Split(":");
                if (user_session_key_segments.Length != 3)
                    continue;

                user_session_data = await _mem_cashe.GetStringValueAsync(key_session);
                data_segments = user_session_data.Split("|");
                if (data_segments.Length != 2)
                    continue;

                res.Add(new SessionStorageModel()
                {
                    DateTimeOfBirth = DateTime.Parse(data_segments[0]),
                    ClientAddress = data_segments[1],
                    Token = user_session_key_segments[2]
                });
            }
            return res.OrderByDescending(x => x.DateTimeOfBirth).ToList();
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
                int seconds_session = SessionMarker.IsLongTimeSession ? _config.Value.CookiesConfig.LongSessionCookieExpiresSeconds : _config.Value.CookiesConfig.SessionCookieExpiresSeconds;
                await _mem_cashe.UpdateValueAsync(UsersAuthenticateService.PrefRedisSessions, SessionMarker.Token, SessionMarker.ToString(), TimeSpan.FromSeconds(seconds_session));
                string? d_session = await _mem_cashe.GetStringValueAsync(new MemCashePrefixModel("sessions", SessionMarker.Login), SessionMarker.Token);
                if (string.IsNullOrEmpty(d_session))
                {
                    d_session = $"{DateTime.Now}|{_http_context.HttpContext.Connection.RemoteIpAddress}";
                }
                await _mem_cashe.UpdateValueAsync(new MemCashePrefixModel("sessions", SessionMarker.Login), SessionMarker.Token, d_session, TimeSpan.FromSeconds(seconds_session + 60));
            }
        }

        public Guid ReadTokenFromRequest()
        {
            if (_http_context.HttpContext.Request.Headers.TryGetValue(_config.Value.CookiesConfig.SessionTokenName, out StringValues tok))
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
