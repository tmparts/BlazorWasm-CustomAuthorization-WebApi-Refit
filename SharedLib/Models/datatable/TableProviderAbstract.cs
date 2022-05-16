﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib.Models
{
    /// <summary>
    /// Абстракция для провайдера таблиц
    /// </summary>
    public abstract class TableProviderAbstract
    {
        /// <summary>
        /// Имя контроллера (шаблон ссылки)
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Номер п/п (по порядку) строк таблицы
        /// </summary>
        public int SequenceStartNum { get; set; }

        /// <summary>
        /// Данные таблици
        /// </summary>
        protected TableDataModel TableData { get; set; }

        /// <summary>
        /// В таблице есть строки
        /// </summary>
        public bool RowsAny => TableData?.Body?.Rows?.Any() == true;

        /// <summary>
        /// В таблиуе есть колонки в головной части
        /// </summary>
        public bool ColumnsAny => TableData?.Head?.Columns.Any() == true;

        /// <summary>
        /// Колонки (головная часть)
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TableDataColumnModel> Columns() => TableData.Head.Columns;

        /// <summary>
        /// Строки таблици
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TableDataRowModel> Rows() => TableData.Body.Rows;

        /// <summary>
        /// Сортировка (от большего к меньшему или от меньшего к большему)
        /// </summary>
        public VerticalDirectionsEnum SortingDirection { get; set; }

        /// <summary>
        /// Имя поля по которому должна происходить сортировка
        /// </summary>
        public string SortBy { get; set; }

        /// <summary>
        /// Определить сортировку колонки
        /// </summary>
        /// <param name="column_name">Имя колонки для проверки/определения</param>
        /// <returns>Направление сортировки, если в даннй момент определена сортировка по определяемой колонке, в противном случае - null</returns>
        public VerticalDirectionsEnum? DetectSort(string column_name)
        {
            if (!string.IsNullOrWhiteSpace(column_name) && !string.IsNullOrWhiteSpace(SortBy) && column_name.ToLower() == SortBy.ToLower())
                return SortingDirection;

            return null;
        }
    }
}
