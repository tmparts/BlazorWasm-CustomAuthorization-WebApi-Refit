////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace LibMetaApp.Models
{
    public class WebConfigModel
    {
        public string[] ClientOrignsCORS { get; set; } = Array.Empty<string>();

        public string AllowedHosts { get; set; } = "*";

        /// <summary>
        /// Maximum number of open connections. When set to null, the number of connections is unlimited.
        /// </summary>
        public long? MaxConcurrentConnections { get; set; } = 100;

        /// <summary>
        /// Maximum allowed size of any request body in bytes. This limit has no effect on upgraded connections which are always unlimited.
        /// This can be overridden per-request via IHttpMaxRequestBodySizeFeature. Defaults to 30,000,000 bytes, which is approximately 28.6MB.
        /// </summary>
        public int MaxRequestBodySize { get; set; } = 30000000;

        /// <summary>
        /// Keep-alive timeout in seconds. Defaults to 120 sec (2 minutes)
        /// </summary>
        public int KeepAliveTimeout { get; set; } = 120;

        /// <summary>
        /// Maximum allowed size for the HTTP request headers. Defaults to 32,768 bytes (32 KB)
        /// </summary>
        public int MaxRequestHeadersTotalSize { get; set; } = 32768;

        /// <summary>
        /// Maximum allowed size for the HTTP request line. Defaults to 8,192 bytes (8 KB).
        /// </summary>
        public int MaxRequestLineSize { get; set; } = 8192;
    }
}