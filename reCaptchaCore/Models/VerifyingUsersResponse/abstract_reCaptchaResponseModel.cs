////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////
using Newtonsoft.Json;
using reCaptcha.stat;
using System;

namespace reCaptcha.Models.VerifyingUsersResponse
{
    public abstract class abstract_reCaptchaResponseModel
    {
        /// <summary>
        /// был ли этот запрос действительным маркером reCAPTCHA для вашего сайта
        /// </summary>
        public bool success { get; set; } = false;

        /// <summary>
        /// timestamp загрузки проверки (ISO format yyyy-MM-dd'T'HH:mm:ssZZ)
        /// </summary>
        public DateTime challenge_ts { get; set; }

        [JsonProperty("error-codes")]
        public string[] ErrorСodes { get; set; }

        public override string ToString()
        {
            string ret_val = (success ? "Success" : "Not success") + " - " + challenge_ts.ToString() + Environment.NewLine;
            if (ErrorСodes != null)
                foreach (string s in ErrorСodes)
                    ret_val += "  ERR:" + (reCaptchaVerifyingErrorCodes.AvailableCodes.ContainsKey(s) ? reCaptchaVerifyingErrorCodes.AvailableCodes[s] : s);

            return ret_val;
        }
    }
}
