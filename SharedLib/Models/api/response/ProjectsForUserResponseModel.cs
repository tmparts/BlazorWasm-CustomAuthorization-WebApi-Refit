////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib.Models
{
    /// <summary>
    /// Проекты пользователя (ответ api/rest)
    /// </summary>
    public class ProjectsForUserResponseModel : PaginationResponseModel
    {
        /// <summary>
        /// Проекты в которых учавствует пользователь
        /// </summary>
        public LinkToProjectForUserModel[] RowsData { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ProjectsForUserResponseModel() { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="init_object">Объект инициализации пагинатора</param>
        public ProjectsForUserResponseModel(PaginationRequestModel init_object) : base(init_object) { }
    }
}
