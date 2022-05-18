////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib.Models;

namespace SharedLib.ClientServices.refit
{
    /// <summary>
    /// REST служба раоты с API пользовательских проектов
    /// </summary>
    public interface IUsersProjectsRestService
    {
        public Task<FindUsersProjectsResponseModel> GetMyProjectsAsync(PaginationRequestModel pagination);
        public Task<UserProjectResponseModel> GetProjectAsync(int id);
    }
}
