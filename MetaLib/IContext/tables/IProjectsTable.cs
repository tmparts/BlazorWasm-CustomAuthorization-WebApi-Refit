////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using DbcMetaLib.Confirmations;
using MetaLib.Models;

namespace DbcMetaLib.Projects
{
    /// <summary>
    /// Интерфейс доступа к проектам
    /// </summary>
    public interface IProjectsTable : SavingChanges
    {        
        public Task AddAsync(ProjectModelDB project, bool auto_save = true);

        public Task UpdateAsync(ProjectModelDB project, bool auto_save = true);
    }
}
