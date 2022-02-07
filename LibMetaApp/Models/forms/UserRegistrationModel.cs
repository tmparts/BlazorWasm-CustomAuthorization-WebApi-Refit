////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace LibMetaApp.Models
{
    public class UserRegistrationModel : UserAuthorizationLiteModel
    {
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Некорректный адрес")]
        public string Email { get; set; } = string.Empty;

        public string PublicName { get; set; } = string.Empty;
    }
}
