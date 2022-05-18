////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib.Models;

namespace SharedLib.ClientServices.refit
{
    /// <summary>
    /// REST служба раоты с API ссылок на проекты
    /// </summary>
    public interface ILinksProjectsRestService
    {
        public Task<GetLinksProjectsResponseModel> GetLinksUsersByProject(int project_id);
    }
}
