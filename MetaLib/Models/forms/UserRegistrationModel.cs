////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib;
using System.ComponentModel.DataAnnotations;

namespace MetaLib.Models
{
    public class UserRegistrationModel : UserAuthorizationLiteModel
    {
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Некорректный адрес Email")]
        [Unlike(nameof(PublicName), ErrorMessage = "Email не может совпадать с публичным именем")]
        [Required]
        public string Email { get; set; } = string.Empty;

        [Unlike(nameof(Login), ErrorMessage = "Публичное имя не может совпадать с приватным логином")]
        [Required]
        public string PublicName { get; set; } = string.Empty;
    }
}
