﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace MetaLib.Models
{
    public class UserRestoreModel
    {
        public string Login { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string ResponseReCAPTCHA { get; set; } = string.Empty;
    }
}
