////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib.Models
{
    /// <summary>
    /// Группа свойств документа
    /// </summary>
    public class DocumentPropertiesGroupModelDB : RealTypeModel
    {
        /// <summary>
        /// Связанные свойства документа
        /// </summary>
        public IEnumerable<DocumentPropertyModelDB> DocumentProperties { get; set; }
    }
}