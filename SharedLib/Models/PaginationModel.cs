using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib.Models
{
    /// <summary>
    /// Базовая модель запроса с поддержкой пагинации
    /// </summary>
    public class PaginationRequestModel
    {
        /// <summary>
        /// Размер страницы (количество элементов на странице)
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Номер текущей страницы
        /// </summary>
        public int PageNum { get; set; }
    }

    /// <summary>
    /// Базовая модель ответа с поддержкой пагинации
    /// </summary>
    public class PaginationResponseModel : PaginationRequestModel
    {
        /// <summary>
        /// Общее/всего количество элементов
        /// </summary>
        public int TotalCount { get; set; }
    }
}
