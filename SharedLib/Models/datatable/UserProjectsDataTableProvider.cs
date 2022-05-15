////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib.Models
{
    /// <summary>
    /// Провайдер таблицы пользовательских проектов
    /// </summary>
    public class UserProjectsDataTableProvider : DataTableProviderAbstract
    {
        /// <summary>
        /// Контруктор
        /// </summary>
        /// <param name="data_table">Данные таблицы</param>
        /// <param name="template_url">Шаблон URI</param>
        public UserProjectsDataTableProvider(ProjectForUserResponseModel data_table, string template_url)
        {
            this.TemplateUrl = template_url;
            List<TableDataColumnModel> сolumns = new List<TableDataColumnModel>()
            {
                new TableDataColumnModel()
                {
                    ColumnDataName = nameof(ProjectForUserModel.Id),
                    SortingDirection = DetectSort(nameof(ProjectForUserModel.Id), data_table.SortBy, data_table.SortingDirection),
                    Title = "Id",
                    Style = " width: 1%; white-space: nowrap;"
                },
                new TableDataColumnModel()
                {
                    ColumnDataName = nameof(ProjectForUserModel.Name),
                    SortingDirection = DetectSort(nameof(ProjectForUserModel.Name), data_table.SortBy, data_table.SortingDirection),
                    Title = "Название"
                },
                new TableDataColumnModel()
                {
                    ColumnDataName = nameof(ProjectForUserModel.AccessLevelUser),
                    SortingDirection = DetectSort(nameof(ProjectForUserModel.AccessLevelUser), data_table.SortBy, data_table.SortingDirection),
                    Title = "Доступ"
                }
            };
            SequenceStartNum = ((data_table.PageNum - 1) * data_table.PageSize) + 1;
            TableData = new TableDataModel(сolumns);
            TableDataRowModel data_row;
            foreach (ProjectForUserModel? row in data_table.RowsData)
            {
                data_row = new TableDataRowModel()
                {
                    IsDeleted = row.IsDeleted,
                    Id = row.Id
                };

                data_row.Cells = new TableDataCellModel[]
                {
                    new TableDataCellModel() { DataCellValue = $"#{row.Id}" },
                    new TableDataCellModel() { DataCellValue = row.Name },
                    new TableDataCellModel() { DataCellValue = row.AccessLevelUser }
                };
                TableData.AddRow(data_row);
            }
        }
    }
}