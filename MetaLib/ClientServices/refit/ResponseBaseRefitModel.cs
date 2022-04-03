using MetaLib.Models;
using System.Net;

namespace MetaLib.ClientServices.refit
{
    public class ResponseBaseRefitModel: ResponseBaseModel, IRefitResponseModel
    {
        public HttpStatusCode? StatusCode { get; set; }
        public Exception Error { get; set; }
        HttpStatusCode? IRefitResponseModel.StatusCode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
