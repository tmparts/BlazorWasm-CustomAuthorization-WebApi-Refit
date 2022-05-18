////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib.Models
{
    /// <summary>
    /// Базовая модель ответа с поддержкой пагинации
    /// </summary>
    public class PaginationResponseModel : PaginationRequestModel
    {
        /// <summary>
        /// Общее/всего количество элементов
        /// </summary>
        public int TotalRowsCount { get; set; }

        /// <summary>
        /// Количесвто страниц пагинатора
        /// </summary>
        /// <param name="default_page_size">Размер страницы по умолчанию на случай если PageSize == 0</param>
        /// <returns>Количесвто страниц пагинатора</returns>
        public uint TotalPagesCount(uint default_page_size = 10)
        {
            if (PageSize == 0)
                return (uint)Math.Ceiling((double)TotalRowsCount / (double)default_page_size);

            return (uint)Math.Ceiling((double)TotalRowsCount / (double)PageSize);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public PaginationResponseModel() { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="init_object">Объект инициализации пагинатора</param>
        public PaginationResponseModel(PaginationRequestModel init_object) : base(init_object) { }
    }
}
