////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////
using System.ComponentModel.DataAnnotations;

namespace reCaptcha.Models
{
    public class reCaptchaVerifyRequestModel
    {
        [Required]
        public string Action { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
