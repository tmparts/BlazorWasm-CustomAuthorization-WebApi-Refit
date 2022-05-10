////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib.Models
{
    /// <summary>
    /// Проект пользователя
    /// </summary>
    public class ProjectForUserModel : EntryDescriptionModel
    {
        /// <summary>
        /// Уровень доступа пользвателя к проекту
        /// </summary>
        public AccessLevelsUsersToProjectsEnum AccessLevelUser { get; set; } = AccessLevelsUsersToProjectsEnum.Blocked;

        /// <summary>
        /// Приведение типов
        /// </summary>
        /// <param name="v">Исходный объект</param>
        public static explicit operator ProjectForUserModel(UserToProjectLinkModelDb v)
        {
            return v is null
                ? null
                : new ProjectForUserModel()
                {
                    Id = v.ProjectId,
                    Description = v.Project.Description,
                    Name = v.Project.Name,
                    IsDeleted = v.IsDeleted,
                    AccessLevelUser = v.AccessLevelUser
                };
        }
    }
}
