////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;
using SharedLib.Models;

namespace SharedLib.Services
{
    /// <summary>
    /// Базовыц компонент для поддержки пагинации
    /// </summary>
    public abstract class PaginationBaseModel : BlazorBusyComponentBaseModel
    {
        protected ClientConfigModel _conf;

        protected override async void OnInitialized()
        {
            if (PageSize.GetValueOrDefault(0) < _conf.PaginationPageSizeMin)
                PageSize = _conf.PaginationPageSizeMin;

            if (PageNum.GetValueOrDefault(0) <= 0)
                PageNum = 1;

            if (string.IsNullOrWhiteSpace(SortBy))
                SortBy = nameof(ProjectForUserModel.Id);

            if (SortingDirection is null)
                SortingDirection = VerticalDirectionsEnum.Down.ToString();
        }

        /// <summary>
        /// Текущая пагинация
        /// </summary>
        public PaginationRequestModel Pagination => new PaginationRequestModel()
        {
            PageNum = PageNum.Value,
            PageSize = PageSize.Value,
            SortBy = SortBy,
            SortingDirection = SortingDirection is null ? VerticalDirectionsEnum.Up : Enum.Parse<VerticalDirectionsEnum>(SortingDirection)
        };

        /// <summary>
        /// Размер страницы пагинатора
        /// </summary>
        [Parameter]
        [SupplyParameterFromQuery]
        public int? PageSize { get; set; }

        /// <summary>
        /// Номер страницы пагинатора
        /// </summary>
        [Parameter]
        [SupplyParameterFromQuery]
        public int? PageNum { get; set; }

        /// <summary>
        /// Направление сортировки пагинатора
        /// </summary>
        [Parameter]
        [SupplyParameterFromQuery]
        public string? SortingDirection { get; set; }

        /// <summary>
        /// Имя поля/колонки/столбца сортировки пагинатора
        /// </summary>
        [Parameter]
        [SupplyParameterFromQuery]
        public string SortBy { get; set; } = "Id";
    }
}