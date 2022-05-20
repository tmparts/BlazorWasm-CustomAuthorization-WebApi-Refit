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
                    ColumnDataName = nameof(LinkToProjectForUserModel.Id),
                    SortingDirection = DetectSort(nameof(LinkToProjectForUserModel.Id)),
                    Title = "Id",
                    Style = " width: 1%; white-space: nowrap;"
                },
                new TableDataColumnModel()
                {
                    ColumnDataName = nameof(LinkToProjectForUserModel.Name),
                    SortingDirection = DetectSort(nameof(LinkToProjectForUserModel.Name)),
                    Title = "Название"
                },
                new TableDataColumnModel()
                {
                    ColumnDataName = nameof(LinkToProjectForUserModel.AccessLevelUser),
                    SortingDirection = DetectSort(nameof(LinkToProjectForUserModel.AccessLevelUser)),
                    Title = "Доступ"
                }
            };
            SequenceStartNum = ((projects_for_user_api_response.PageNum - 1) * projects_for_user_api_response.PageSize) + 1;
            TableData = new TableDataModel(сolumns);
            TableDataRowModel data_row;
            foreach (LinkToProjectForUserModel? row in projects_for_user_api_response.RowsData)
            {
                data_row = new TableDataRowModel()
                {
                    IsDeleted = row.IsDeleted || row.ProjectIsDeleted.GetValueOrDefault(false),
                    Id = row.ProjectId
                };
                string del_info = row.IsDeleted ? "ссылка" : "";
                if (row.ProjectIsDeleted.HasValue)
                {
                    del_info += row.ProjectIsDeleted.Value ? " проект" : "";
                }
                data_row.Cells = new TableDataCellModel[]
                {
                    new TableDataCellModel() { DataCellValue = $"#{row.ProjectId}" },
                    new TableDataCellModel() { DataCellValue = $"{row.Name}{(string.IsNullOrWhiteSpace(del_info) ? "" : $" (удалены: {del_info.Trim().Replace(" "," и ")})")}" },
                    new TableDataCellModel() { DataCellValue = row.AccessLevelUser }
                };
                TableData.AddRow(data_row);
            }
        }
    }
}