////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace MetaLib.Services
{
    public class RefitHeadersDelegatingHandler : DelegatingHandler
    {
        private SessionMarkerLiteModel _marker { get; set; }
        private readonly IMemoryCache _memory_cache;
        private readonly ClientConfigModel _config;

        public RefitHeadersDelegatingHandler(SessionMarkerLiteModel set_marker, IMemoryCache set_memory_cache, ClientConfigModel set_config)
        {
            _marker = set_marker;
            _memory_cache = set_memory_cache;
            _config = set_config;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(_marker.Token))
            {
                _memory_cache.Set(_config.CookiesConfig.SessionTokenName, _marker.Token,new DateTimeOffset(DateTime.Now.AddSeconds(_config.CookiesConfig.LongSessionCookieExpiresSeconds)));
                request.Headers.Add(_config.CookiesConfig.SessionTokenName, _marker.Token);
            }
            return await base.SendAsync(request, cancellationToken);
        }

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(_marker.Token))
            {
                _memory_cache.Set(_config.CookiesConfig.SessionTokenName, _marker.Token, new DateTimeOffset(DateTime.Now.AddSeconds(_config.CookiesConfig.LongSessionCookieExpiresSeconds)));
                request.Headers.Add(_config.CookiesConfig.SessionTokenName, _marker.Token);
            }
            return base.Send(request, cancellationToken);
        }
    }
}
