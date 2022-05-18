////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Refit;
using SharedLib.Models;

namespace SharedLib.Services
{
    /// <summary>
    /// Refit коннектор к API/UsersProjects
    /// </summary>
    [Headers("Content-Type: application/json")]
    public interface IUsersProjectsRefitService
    {
        [Get("/api/usersprojects")]
        Task<ApiResponse<FindUsersProjectsResponseModel>> GetMyProjectsAsync(PaginationRequestModel pagination);

        [Get("/api/usersprojects/{id}")]
        Task<ApiResponse<UserProjectResponseModel>> GetProjectAsync(int id);
    }
}
