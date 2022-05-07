////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLib.Models
{
    /// <summary>
    /// Поле/свойство объекта (базовая модель)
    /// </summary>
    public abstract class MetaMapBaseModelDB : RealTypeModel
    {
        /// <summary>
        /// Расположение свойства/поля (оснвоное тело объекта или табличная часть)
        /// </summary>
        public AreasPropertiesEnum AreaProperty { get; set; }

        /// <summary>
        /// Тип поля (перечисление, справочник, документ)
        /// </summary>
        public PropertyTypesEnum PropertyType { get; set; }

        /// <summary>
        /// Порядок/сортировка полей
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Sorting { get; set; }
    }
}