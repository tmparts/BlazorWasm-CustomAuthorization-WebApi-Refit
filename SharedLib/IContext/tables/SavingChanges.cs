////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib
{
    /// <summary>
    /// Поддержка сохранения данных в БД
    /// </summary>
    public interface SavingChanges
    {
        /// <summary>
        /// Сохранить текущие изменения в БД
        /// </summary>
        /// <returns>Количество строк затронутых (или созданных) объектов/строк данных</returns>
        public Task<int> SaveChangesAsync();
    }
}
