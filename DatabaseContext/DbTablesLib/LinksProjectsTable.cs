////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using DbcLib;
using SharedLib.Models;
using Microsoft.Extensions.Logging;
using SharedLib;
using Microsoft.Extensions.Options;
using System.Data.Entity;

namespace DbcMetaSqliteLib.Projects
{
    /// <summary>
    /// Доступ к таблице базы данных ссылок пользователей на проекты
    /// </summary>
    public class LinksProjectsTable : ILinksProjectsTable
    {
        readonly DbAppContext _db_context;
        readonly ILogger<LinksProjectsTable> _logger;
        readonly IOptions<ServerConfigModel> _config;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="set_db_context"></param>
        /// <param name="set_logger"></param>
        public LinksProjectsTable(DbAppContext set_db_context, ILogger<LinksProjectsTable> set_logger, IOptions<ServerConfigModel> set_config)
        {
            _db_context = set_db_context;
            _logger = set_logger;
            _config = set_config;
        }

        /// <summary>
        /// Получить ссылки на проект
        /// </summary>
        /// <param name="project_id">Идентификатор проекта</param>
        /// <param name="include_user_data">Загрузить данные по пользователям</param>
        /// <returns>Сссылки на проект</returns>
        public async Task<IEnumerable<UserToProjectLinkModelDb>> GetLinksUsersByProject(int project_id, bool include_user_data = true)
        {
            IQueryable<UserToProjectLinkModelDb> query = _db_context.UsersToProjectsLinks.Where(x => x.ProjectId == project_id);
            if (include_user_data)
            {
                query = query.Include(x => x.User);
            }
            return await query.ToArrayAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db_context.SaveChangesAsync();
        }
    }
}