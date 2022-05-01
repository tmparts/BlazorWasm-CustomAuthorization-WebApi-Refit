////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using DbcLib;
using SharedLib.Models;
using Microsoft.Extensions.Logging;
using SharedLib;

namespace DbcMetaSqliteLib.Projects
{
    /// <summary>
    /// Доступ к таблице проектов базы данных SQLite
    /// </summary>
    public class ProjectsTable : IProjectsTable
    {
        readonly DbAppContext _db_context;
        readonly ILogger<ProjectsTable> _logger;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="set_db_context"></param>
        /// <param name="set_logger"></param>
        public ProjectsTable(DbAppContext set_db_context, ILogger<ProjectsTable> set_logger)
        {
            _db_context = set_db_context;
            _logger = set_logger;
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
    }
}