////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib.Models
{
    /// <summary>
    /// Проекты пользователя
    /// </summary>
    public class ProjectForUserResponseModel : PaginationResponseModel
    {
        /// <summary>
        /// Проекты в которых учавствует пользователь
        /// </summary>
        public ProjectForUserModel[] RowsData { get; set; }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ProjectForUserResponseModel() { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="init_object">Объект инициализации пагинатора</param>
        public ProjectForUserResponseModel(PaginationRequestModel init_object) : base(init_object) { }
    }
}
