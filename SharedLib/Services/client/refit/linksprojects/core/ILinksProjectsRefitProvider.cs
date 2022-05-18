////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib.Models;
using Refit;

namespace SharedLib.Services
{
    public interface ILinksProjectsRefitProvider
    {
        Task<ApiResponse<GetLinksProjectsResponseModel>> GetLinksUsersByProject(int project_id);
    }
}
