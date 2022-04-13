using System.ComponentModel.DataAnnotations;

namespace MetaLib.Models
{
    public class PasswordsPairModel
    {
        [Required(ErrorMessage = "Укажите текущий пароль")]
        public string PasswordCurrent { get; set; } = string.Empty;

        [StringLength(30, MinimumLength = 8, ErrorMessage = "Длина пароля должна быть от 8 до 30 символов")]
        [Unlike(nameof(PasswordCurrent), ErrorMessage = "Новый пароль совпадает со старым")]
        public string PasswordNew { get; set; } = string.Empty;

        [Compare(nameof(PasswordNew), ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; } = string.Empty;
    }
}
