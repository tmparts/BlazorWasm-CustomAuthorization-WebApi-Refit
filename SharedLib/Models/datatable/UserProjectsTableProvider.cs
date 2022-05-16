////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib.Models
{
    /// <summary>
    /// Провайдер таблицы пользовательских проектов
    /// </summary>
    public class UserProjectsTableProvider : TableProviderAbstract
    {
        /// <summary>
        /// Контруктор: Провайдер таблицы пользовательских проектов
        /// </summary>
        /// <param name="projects_for_user_api_response">Данные таблицы</param>
        /// <param name="controller_name">Имя контроллера (Шаблон URI)</param>
        public UserProjectsTableProvider(ProjectsForUserResponseModel projects_for_user_api_response, string controller_name)
        {
            ControllerName = controller_name;
            SortingDirection = projects_for_user_api_response.SortingDirection;
            SortBy = projects_for_user_api_response.SortBy;

            List<TableDataColumnModel> сolumns = new List<TableDataColumnModel>()
            {
                new TableDataColumnModel()
                {
                    ColumnDataName = nameof(ProjectForUserModel.Id),
                    SortingDirection = DetectSort(nameof(ProjectForUserModel.Id)),
                    Title = "Id",
                    Style = " width: 1%; white-space: nowrap;"
                },
                new TableDataColumnModel()
                {
                    ColumnDataName = nameof(ProjectForUserModel.Name),
                    SortingDirection = DetectSort(nameof(ProjectForUserModel.Name)),
                    Title = "Название"
                },
                new TableDataColumnModel()
                {
                    ColumnDataName = nameof(ProjectForUserModel.AccessLevelUser),
                    SortingDirection = DetectSort(nameof(ProjectForUserModel.AccessLevelUser)),
                    Title = "Доступ"
                }
            };
            SequenceStartNum = ((projects_for_user_api_response.PageNum - 1) * projects_for_user_api_response.PageSize) + 1;
            TableData = new TableDataModel(сolumns);
            TableDataRowModel data_row;
            foreach (ProjectForUserModel? row in projects_for_user_api_response.RowsData)
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