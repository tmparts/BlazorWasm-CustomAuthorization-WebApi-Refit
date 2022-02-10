////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using System.ComponentModel.DataAnnotations;

namespace LibMetaApp.Models
{
    public class UserRestoreModel
    {
        public string Login { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string ResponseReCAPTCHA { get; set; } = string.Empty;
    }
}
