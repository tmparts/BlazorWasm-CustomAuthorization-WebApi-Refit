using MetaLib.Models;
using System.Net;

namespace MetaLib.ClientServices.refit
{
    public class AuthUserResponseRefitModel : AuthUserResponseModel, IRefitResponseModel
    {
        public HttpStatusCode? StatusCode { get; set; }
        public Exception Error { get; set; }
    }
}
