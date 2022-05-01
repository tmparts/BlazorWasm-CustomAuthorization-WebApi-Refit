////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Microsoft.EntityFrameworkCore;

namespace SharedLib.Models
{
    /// <summary>
    /// Модель пользователя (полная)
    /// </summary>
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(Login), IsUnique = true)]

    [Index(nameof(ConfirmationType))]
    [Index(nameof(AccessLevelUser))]
    public class UserModelDB : UserMediumModel
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public UserModelDB() { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя пользователя</param>
        public UserModelDB(string name) : base(name) { }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Имя пользователя</param>
        /// <param name="access_level">Уровень доступа пользователя</param>
        public UserModelDB(string name, AccessLevelsUsersEnum access_level) : base(name) { AccessLevelUser = access_level; }

        /// <summary>
        /// Хешь пароля пользователя
        /// </summary>
        public string PasswordHash { get; set; } = string.Empty;

        /// <summary>
        /// Оператор преобразования/приведения типов
        /// </summary>
        /// <param name="v"></param>
        public static explicit operator UserModelDB(UserRegistrationModel v)
        {
            return new UserModelDB()
            {
                AccessLevelUser = AccessLevelsUsersEnum.Auth,
                Email = v.Email,
                Login = v.Login,
                Name = v.PublicName,
                PasswordHash = GlobalUtils.CalculateHashString(v.Password)
            };
        }
    }
}