using MetaLib.Models;
using System.Net;

namespace MetaLib.ClientServices.refit
{
    public class GetUserProfileResponseRefitModel : GetUserProfileResponseModel, IRefitResponseModel
    {
        public HttpStatusCode? StatusCode { get; set; }
        public Exception Error { get; set; }
    }
}