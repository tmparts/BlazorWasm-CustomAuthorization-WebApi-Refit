////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using System.Net;

namespace MetaLib.Services
{
    public class RefitHeadersDelegatingHandler : DelegatingHandler
    {
        private SessionMarkerLiteModel _marker { get; set; }
        private readonly IJSRuntime _js_runtime;
        private readonly ClientConfigModel _config;

        public RefitHeadersDelegatingHandler(SessionMarkerLiteModel set_marker, IJSRuntime set_js_runtime, ClientConfigModel set_config)
        {
            _marker = set_marker;
            _js_runtime = set_js_runtime;
            _config = set_config;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(_marker.Token))
            {
                request.Headers.Add(_config.CookiesConfig.SessionTokenName, _marker.Token);
            }
            HttpResponseMessage? response = await base.SendAsync(request, cancellationToken);

#if DEBUG
            var resp_headers = response.Headers.ToArray();
#endif

            await _js_runtime.InvokeVoidAsync("methods.UpdateCookie", GlobalStaticConstants.SESSION_STORAGE_KEY_USER_ID, _config.CookiesConfig.LongSessionCookieExpiresSeconds, "/");
            await _js_runtime.InvokeVoidAsync("methods.UpdateCookie", GlobalStaticConstants.SESSION_STORAGE_KEY_LOGIN, _config.CookiesConfig.LongSessionCookieExpiresSeconds, "/");
            await _js_runtime.InvokeVoidAsync("methods.UpdateCookie", GlobalStaticConstants.SESSION_STORAGE_KEY_LEVEL,  _config.CookiesConfig.LongSessionCookieExpiresSeconds, "/");
            await _js_runtime.InvokeVoidAsync("methods.UpdateCookie", GlobalStaticConstants.SESSION_STORAGE_KEY_TOKEN,  _config.CookiesConfig.LongSessionCookieExpiresSeconds, "/");

            return response;
        }
    }
}
