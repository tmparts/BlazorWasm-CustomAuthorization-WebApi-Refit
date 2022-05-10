////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib.Models;
using Refit;

namespace SharedLib.Services
{
    public interface IUsersProjectsRefitProvider
    {
        Task<ApiResponse<FindUsersProjectsResponseModel>> GetMyProjectsAsync(PaginationRequestModel pagination);
    }
}
