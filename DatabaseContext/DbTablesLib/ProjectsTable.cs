////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using DbcLib;
using SharedLib.Models;
using Microsoft.Extensions.Logging;
using SharedLib;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DbcMetaSqliteLib.Projects
{
    /// <summary>
    /// Доступ к таблице проектов базы данных
    /// </summary>
    public class ProjectsTable : IProjectsTable
    {
        readonly DbAppContext _db_context;
        readonly ILogger<ProjectsTable> _logger;
        readonly IOptions<ServerConfigModel> _config;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="set_db_context"></param>
        /// <param name="set_logger"></param>
        public ProjectsTable(DbAppContext set_db_context, ILogger<ProjectsTable> set_logger, IOptions<ServerConfigModel> set_config)
        {
            _db_context = set_db_context;
            _logger = set_logger;
            _config = set_config;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db_context.SaveChangesAsync();
        }

        public async Task AddAsync(ProjectModelDB project, bool auto_save = true)
        {
            await _db_context.AddAsync(project);
            if (auto_save)
                await _db_context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProjectModelDB project, bool auto_save = true)
        {
            _db_context.Update(project);
            if (auto_save)
                await _db_context.SaveChangesAsync();
        }

        /// <summary>
        /// Удалить проект
        /// </summary>
        /// <param name="project_id">Идентификатор проекта</param>
        /// <param name="auto_save"></param>
        /// <returns>Автоматически сохранить изменения в БД</returns>
        public async Task<bool> DeleteAsync(int project_id, bool auto_save = true)
        {
            ProjectModelDB? project_db = await _db_context.Projects.FirstOrDefaultAsync(x => x.Id == project_id);
            if (project_db is null)
                return false;

            _db_context.Remove(project_db);

            if (auto_save)
                await _db_context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Получить проекты для пользователя
        /// </summary>
        /// <param name="user">Пользователь, для которого производится поиск</param>
        /// <param name="pagination">Настройки пагинатора</param>
        /// <returns>Набор проектов для пользователя</returns>
        public async Task<ProjectForUserResponseModel> GetProjectsForUserAsync((int Id, AccessLevelsUsersEnum AccessLevelUser) user, PaginationRequestModel pagination)
        {
            ProjectForUserResponseModel res;
            if (pagination is null)
                res = new ProjectForUserResponseModel();
            else
                res = new ProjectForUserResponseModel(pagination);

            if (res.PageSize <= _config.Value.PaginationPageSizeMin)
            {
                _logger.LogError(new ArgumentOutOfRangeException(nameof(res.PageSize)), $"Размер страницы пагинатора ={res.PageSize}. Этот параметр не может быть меньше {_config.Value.PaginationPageSizeMin}");
                res.PageSize = _config.Value.PaginationPageSizeMin;
            }

            if (res.PageNum <= 0)
            {
                res.PageNum = 1;
            }

            IQueryable<UserToProjectLinkModelDb> query = _db_context.UsersToProjectsLinks.Where(x => x.UserId == user.Id && (user.AccessLevelUser >= AccessLevelsUsersEnum.Manager || !x.IsDeleted)).Include(x => x.Project);

            res.TotalRowsCount = query.Count();

            switch (res.SortBy)
            {
                case nameof(UserToProjectLinkModelDb.AccessLevelUser):
                    query = res.SortingDirection == VerticalDirectionsEnum.Up
                        ? query.OrderByDescending(x => x.AccessLevelUser)
                        : query.OrderBy(x => x.AccessLevelUser);
                    break;
                case nameof(UserToProjectLinkModelDb.Project.Name):
                    query = res.SortingDirection == VerticalDirectionsEnum.Up
                        ? query.OrderByDescending(x => x.Project.Name)
                        : query.OrderBy(x => x.Project.Name);
                    break;
                default:
                    query = res.SortingDirection == VerticalDirectionsEnum.Up
                        ? query.OrderByDescending(x => x.ProjectId)
                        : query.OrderBy(x => x.ProjectId);
                    break;
            }

            query = query.Skip((res.PageNum - 1) * res.PageSize).Take(res.PageSize);
            UserToProjectLinkModelDb[] projects_links = await query.ToArrayAsync();
#if DEBUG
            var v1 = JsonConvert.SerializeObject(projects_links,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        });

#endif
            res.RowsData = projects_links.Select(x => (ProjectForUserModel)x).ToArray();

#if DEBUG
            var v2 = JsonConvert.SerializeObject(res);
#endif

            return res;
        }

        public async Task<ProjectModelDB?> GetProjectForUserAsync(int project_id, int user_id, bool include_sers_data)
        {
            if (project_id <= 0)
            {
                _logger.LogError("Идентификатор проекта не может быть <= 0", new ArgumentOutOfRangeException(nameof(project_id)));
                return null;
            }

            if (user_id <= 0)
            {
                _logger.LogError("Идентификатор пользователя не может быть <= 0", new ArgumentOutOfRangeException(nameof(user_id)));
                return null;
            }

            IQueryable<ProjectModelDB> query = from project in _db_context.Projects
                                               join link in _db_context.UsersToProjectsLinks on project.Id equals link.ProjectId
                                               where link.UserId == user_id && project.Id == project_id
                                               select project;

            if (include_sers_data)
            {
                query = query.Include(x => x.UsersLinks).ThenInclude(x => x.User);
            }

            return await query.FirstOrDefaultAsync();
        }
    }
}