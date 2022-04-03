using MetaLib.Models;
using System.Net;

namespace MetaLib.ClientServices.refit.users.auth.models
{
    public class SessionReadResponseRefitModel : SessionReadResponseModel, IRefitResponseModel
    {
        public HttpStatusCode? StatusCode { get; set; }
        public Exception Error { get; set; }
    }
}
