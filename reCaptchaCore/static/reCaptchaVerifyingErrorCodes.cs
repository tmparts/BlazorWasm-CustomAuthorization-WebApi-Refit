////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using System.Collections.Generic;

namespace reCaptcha.stat
{
    public static class reCaptchaVerifyingErrorCodes
    {
        public static Dictionary<string, string> AvailableCodes = new Dictionary<string, string>()
        {
            {"missing-input-secret", "Секретный параметр отсутствует"},
            {"invalid-input-secret", "Параметр secret является недопустимым или неправильным"},
            {"missing-input-response", "Параметр ответа отсутствует"},
            {"invalid-input-response", "Параметр ответа является недопустимым или неправильным"},
            {"bad-request", "Запрос недействителен или неправильно сформирован"},
            {"timeout-or-duplicate", "Ответ больше не действителен: либо слишком стар, либо использовался ранее"},
            { "invalid-keys","Использование существующего, но неправильного секретного ключа учетной записи"}
        };
    }
}
