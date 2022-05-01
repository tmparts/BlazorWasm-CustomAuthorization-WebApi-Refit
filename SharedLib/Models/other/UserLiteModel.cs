////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace SharedLib.Models
{
    /// <summary>
    /// Пользователь (лёгкая модель)
    /// </summary>
    public class UserLiteModel : EntryCreatedModel
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public UserLiteModel() { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя пользователя</param>
        public UserLiteModel(string name) : base(name) { }

        /// <summary>
        /// Конструткор
        /// </summary>
        /// <param name="name">Имя пользователя</param>
        /// <param name="access_level">Уровень доступа пользователя</param>
        public UserLiteModel(string name, AccessLevelsUsersEnum access_level) : base(name) { AccessLevelUser = access_level; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя пользователя</param>
        /// <param name="access_level">Уровень доступа/прав пользователя</param>
        /// <param name="confirmation_type">Тип подтверждения учётной записи пользователя (по email, вручную и т.п.)</param>
        public UserLiteModel(string name, AccessLevelsUsersEnum access_level, ConfirmationUsersTypesEnum confirmation_type) : base(name) { AccessLevelUser = access_level; ConfirmationType = confirmation_type; }

        /// <summary>
        /// Логин пользователя для входа
        /// </summary>
        public string Login { get; set; } = string.Empty;

        /// <summary>
        /// Email пользователя
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// О пользователе
        /// </summary>
        public string About { get; set; } = string.Empty;

        /// <summary>
        /// Уровень доступа/прав пользователя
        /// </summary>
        public AccessLevelsUsersEnum AccessLevelUser { get; set; } = AccessLevelsUsersEnum.Anonim;

        /// <summary>
        /// Тип подтверждения учётной записи пользователя (по email, вручную и т.п.)
        /// </summary>
        public ConfirmationUsersTypesEnum ConfirmationType { get; set; } = ConfirmationUsersTypesEnum.None;

        /// <summary>
        /// Оператор сравнения пользователей
        /// </summary>
        /// <param name="user1"></param>
        /// <param name="user2"></param>
        /// <returns>Результат сравнения пользователей</returns>
        public static bool operator ==(UserLiteModel user1, UserLiteModel user2)
        {
            return user1.Id == user2.Id
                && user1.AccessLevelUser == user2.AccessLevelUser
                && user1.Email == user2.Email
                && user1.ConfirmationType == user2.ConfirmationType
                && user1.Login == user2.Login
                && user1.Name == user2.Name
                && user1.About == user2.About;
        }
        /// <summary>
        /// Оператор сравнения пользователей
        /// </summary>
        /// <param name="user1"></param>
        /// <param name="user2"></param>
        /// <returns>Результат сравнения пользователей</returns>
        public static bool operator !=(UserLiteModel user1, UserLiteModel user2) => !(user1 == user2);
        /// <summary>
        /// Оператор сравнения пользователей
        /// </summary>
        /// <param name="user1"></param>
        /// <param name="user2"></param>
        /// <returns>Результат сравнения пользователей</returns>
        public static bool operator ==(UserLiteModel user1, UserMediumModel user2)
        {
            return user1.Id == user2.Id
                && user1.AccessLevelUser == user2.AccessLevelUser
                && user1.Email == user2.Email
                && user1.ConfirmationType == user2.ConfirmationType
                && user1.Login == user2.Login
                && user1.Name == user2.Name
                && user1.About == user2.About;
        }
        /// <summary>
        /// Оператор сравнения пользователей
        /// </summary>
        /// <param name="user1"></param>
        /// <param name="user2"></param>
        /// <returns>Результат сравнения пользователей</returns>
        public static bool operator !=(UserLiteModel user1, UserMediumModel user2) => !(user1 == user2);

        /// <summary>
        /// Проверка идентичности/равенсва объектов
        /// </summary>
        /// <param name="obj">Объект для сравнения</param>
        /// <returns>Результат сравнения</returns>
        /// <exception cref="NotImplementedException">Необработанное исключение</exception>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is UserLiteModel)
            {
                return (UserLiteModel)obj == this;
            }

            if (obj is UserMediumModel)
            {
                return (UserMediumModel)obj == this;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            throw new NotImplementedException();
        }
    }
}