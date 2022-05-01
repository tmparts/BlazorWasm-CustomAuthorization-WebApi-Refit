////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using SharedLib.Models.enums;

namespace SharedLib.Models
{
    /// <summary>
    /// Модель поиска по текстовому значению
    /// </summary>
    public class FindTextModel
    {
        /// <summary>
        /// Текст (или шаблон регулярного выражения) для поиска
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Режим поиска
        /// </summary>
        public FindTextModesEnum Mode { get; set; }
    }
}
