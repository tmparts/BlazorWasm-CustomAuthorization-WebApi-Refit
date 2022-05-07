////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib.Models
{
    /// <summary>
    /// Поле/свойтво справочника
    /// </summary>
    public class ReferenceBookPropertyModelDB : MetaMapBaseModelDB
    {
        /// <summary>
        /// Группа свойств справочника (внешний ключ)
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Группа свойств справочника
        /// </summary>
        public ReferenceBookPropertiesGroupModelDB Group { get; set; }
    }
}