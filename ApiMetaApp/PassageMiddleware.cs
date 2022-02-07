////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using LibMetaApp;
using SrvMetaApp.Models;
using System.Globalization;

namespace ApiMetaApp
{
    public class PassageMiddleware
    {
        private readonly RequestDelegate _next;

        public PassageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext _http_context, SessionService _session, ILogger<PassageMiddleware> _logger)
        {
            CultureInfo.CurrentCulture = GlobalUtils.RU;
            CultureInfo.CurrentUICulture = GlobalUtils.RU;
            try
            {
                _session.InitSession();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error PassageMiddleware");
            }

            await _next.Invoke(_http_context);
        }
    }
}
