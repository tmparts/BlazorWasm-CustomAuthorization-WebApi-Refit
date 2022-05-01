////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib.Models
{
    /// <summary>
    /// Пользователь (промежуточная модель)
    /// </summary>
    public class UserMediumModel : UserLiteModel
    {
        /// <summary>
        /// Конструткор
        /// </summary>
        public UserMediumModel() { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя пользователя</param>
        public UserMediumModel(string name) : base(name) { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя пользователя</param>
        /// <param name="access_level">Еровень доступа пользователя</param>
        public UserMediumModel(string name, AccessLevelsUsersEnum access_level) : base(name) { AccessLevelUser = access_level; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя пользователя</param>
        /// <param name="access_level">Уровень доступа пользователя</param>
        /// <param name="confirmation_type">Тип подвтерждения учётной записи пользователя</param>
        public UserMediumModel(string name, AccessLevelsUsersEnum access_level, ConfirmationUsersTypesEnum confirmation_type) : base(name) { AccessLevelUser = access_level; ConfirmationType = confirmation_type; }

        /// <summary>
        /// Группы в которых состояит пользователь
        /// </summary>
        public IEnumerable<UserGroupModelDB>? Groups { get; set; }

        /// <summary>
        /// Проекты в которых учавствует пользователь
        /// </summary>
        public IEnumerable<ProjectModelDB>? Projects { get; set; }
    }
}