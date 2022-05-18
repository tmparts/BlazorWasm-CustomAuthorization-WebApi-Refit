////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib.Models
{
    /// <summary>
    /// Ссылки пользователей на проекты (результат запроса)
    /// </summary>
    public class GetLinksProjectsResponseModel : ResponseBaseModel
    {
        public UserToProjectLinkModelDb[] Links { get; set; }
    }
}