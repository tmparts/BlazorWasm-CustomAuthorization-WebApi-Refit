////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib.Models
{
    /// <summary>
    /// Вещественная модель (базовая)
    /// </summary>
    public abstract class RealTypeModel : EntryDescriptionModel
    {
        /// <summary>
        /// Системное имя (имя типа/класса C#)
        /// </summary>
        public string SystemCodeName { get; set; }
    }
}
