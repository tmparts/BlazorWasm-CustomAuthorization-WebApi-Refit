////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////
using System;

namespace reCaptcha.Models.VerifyingUsersResponse
{
    public class reCaptcha2ResponseModel : abstract_reCaptchaResponseModel
    {
        /// <summary>
        /// имя хоста сайта, на котором была решена рекапча
        /// </summary>
        public string hostname { get; set; }

        public override string ToString()
        {
            return base.ToString() + Environment.NewLine + 
                "Hostname:" + hostname;
        }
    }
}
