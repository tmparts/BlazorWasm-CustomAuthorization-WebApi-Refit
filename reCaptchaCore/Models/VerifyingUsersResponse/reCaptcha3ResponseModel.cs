////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////
using System;

namespace reCaptcha.Models.VerifyingUsersResponse
{
    public class reCaptcha3ResponseModel : reCaptcha2ResponseModel
    {
        /// <summary>
        /// имя действия для этого запроса (важно проверить)
        /// </summary>
        public string action { get; set; }

        /// <summary>
        /// оценка для этого запроса (0.0 - 1.0)
        /// </summary>
        public ushort score { get; set; }

        public override string ToString()
        {
            return base.ToString() + Environment.NewLine + "Action: " + action + Environment.NewLine + "Score: " + score;
        }
    }
}
