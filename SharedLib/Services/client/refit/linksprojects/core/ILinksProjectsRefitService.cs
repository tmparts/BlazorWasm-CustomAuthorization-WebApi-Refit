////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Refit;
using SharedLib.Models;

namespace SharedLib.Services
{
    /// <summary>
    /// Refit коннектор к API/LinksProjects
    /// </summary>
    [Headers("Content-Type: application/json")]
    public interface ILinksProjectsRefitService
    {
        [Get("/api/linksprojects")]
        Task<ApiResponse<GetLinksProjectsResponseModel>> GetLinksUsersByProject(int project_id);
    }
}
