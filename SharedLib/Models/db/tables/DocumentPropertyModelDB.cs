////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib.Models
{
    /// <summary>
    /// Поле/свойтво документа
    /// </summary>
    public class DocumentPropertyModelDB : MetaMapBaseModelDB
    {
        /// <summary>
        /// Группа свойств документа (внешний ключ)
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Группа свойств документа
        /// </summary>
        public DocumentPropertiesGroupModelDB Group { get; set; }
    }
}