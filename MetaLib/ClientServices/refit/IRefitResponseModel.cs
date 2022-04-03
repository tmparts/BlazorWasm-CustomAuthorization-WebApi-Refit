using System.Net;

namespace MetaLib.ClientServices.refit
{
    public interface IRefitResponseModel
    {
        public HttpStatusCode? StatusCode { get; set; }
        public Exception Error { get; set; }
    }
}