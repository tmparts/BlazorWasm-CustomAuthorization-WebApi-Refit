////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace LibMetaApp.Models
{
    public class UserAuthorizationLiteModel
    {
        [StringLength(15, MinimumLength = 4, ErrorMessage = "Длина логина должна быть от 4 до 15 символов")]
        public string Login { get; set; } = string.Empty;

        [StringLength(30, MinimumLength = 8, ErrorMessage = "Длина пароля должна быть от 8 до 30 символов")]
        public string Password { get; set; } = string.Empty;
    }
}
