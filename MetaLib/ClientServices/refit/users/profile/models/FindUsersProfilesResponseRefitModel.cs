using MetaLib.Models;
using System.Net;

namespace MetaLib.ClientServices.refit.users.profile
{
    public class FindUsersProfilesResponseRefitModel : FindUsersProfilesResponseModel, IRefitResponseModel
    {
        public HttpStatusCode? StatusCode { get; set; }
        public Exception Error { get; set; }
    }
}