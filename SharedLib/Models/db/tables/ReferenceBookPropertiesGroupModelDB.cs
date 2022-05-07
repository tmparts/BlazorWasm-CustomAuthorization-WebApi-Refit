////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib.Models
{
    /// <summary>
    /// Группа свойств справочника
    /// </summary>
    public class ReferenceBookPropertiesGroupModelDB : RealTypeModel
    {
        /// <summary>
        /// Связаные свойства справочника
        /// </summary>
        public IEnumerable<ReferenceBookPropertyModelDB> ReferenceBookProperties { get; set; }
    }
}