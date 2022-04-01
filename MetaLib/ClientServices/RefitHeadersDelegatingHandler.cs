////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models;

namespace MetaLib.Services
{
    public class RefitHeadersDelegatingHandler : DelegatingHandler
    {
        private SessionMarkerLiteModel _marker { get; set; }

        public RefitHeadersDelegatingHandler(SessionMarkerLiteModel set_marker)
        {
            _marker = set_marker;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(_marker.Token))
            {
                request.Headers.Add(GlobalStaticConstants.SESSION_TOKEN_NAME, _marker.Token);
            }
            return await base.SendAsync(request, cancellationToken);
        }

        protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(_marker.Token))
            {
                request.Headers.Add(GlobalStaticConstants.SESSION_TOKEN_NAME, _marker.Token);
            }
            return base.Send(request, cancellationToken);
        }
    }
}
