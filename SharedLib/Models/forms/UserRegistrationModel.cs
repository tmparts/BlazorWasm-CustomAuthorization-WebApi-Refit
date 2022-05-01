////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace SharedLib.Models
{
    /// <summary>
    /// Объект (форма) регистрации пользователя
    /// </summary>
    public class UserRegistrationModel : UserAuthorizationLiteModel
    {
        /// <summary>
        /// Повтор пароля пользователя
        /// </summary>
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; } = string.Empty;

        /// <summary>
        /// Email пользователя
        /// </summary>
        [EmailAddress(ErrorMessage = "Некорректный адрес Email")]
        [Unlike(nameof(PublicName), ErrorMessage = "Email не может совпадать с публичным именем")]
        [Required]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Публичное имя пользователя
        /// </summary>
        [Unlike(nameof(Login), ErrorMessage = "Публичное имя не может совпадать с приватным логином")]
        [Required]
        public string PublicName { get; set; } = string.Empty;
    }
}
