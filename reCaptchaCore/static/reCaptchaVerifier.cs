////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////
using Newtonsoft.Json;
using reCaptcha.Models.VerifyingUsersResponse;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace reCaptcha.stat
{
    public static class reCaptchaVerifier
    {
        public static reCaptcha3ResponseModel reCaptcha3SiteVerify(string secret, string response, string remoteip = null)
        {
            byte[] respBytes = reCaptchaSiteVerify(secret, response, remoteip);
            return DeserializeFromStream(new MemoryStream(respBytes), typeof(reCaptcha3ResponseModel)) as reCaptcha3ResponseModel;
        }
        public async static Task<reCaptcha3ResponseModel> reCaptcha3SiteVerifyAsync(string secret, string response, string remoteip = null)
        {
            byte[] respBytes = await RunSave(() => reCaptchaSiteVerify(secret, response, remoteip), Array.Empty<byte>());
            return await RunSave(() => DeserializeFromStream(new MemoryStream(respBytes), typeof(reCaptcha3ResponseModel)) as reCaptcha3ResponseModel, null);
        }

        public static reCaptcha2ResponseModel reCaptcha2SiteVerify(string secret, string response, string remoteip = null)
        {
            byte[] respBytes = reCaptchaSiteVerify(secret, response, remoteip);
            return DeserializeFromStream(new MemoryStream(respBytes), typeof(reCaptcha2ResponseModel)) as reCaptcha2ResponseModel;
        }
        public async static Task<reCaptcha2ResponseModel> reCaptcha2SiteVerifyAsync(string secret, string response, string remoteip = null)
        {
            byte[] respBytes = await RunSave(() => reCaptchaSiteVerify(secret, response, remoteip), Array.Empty<byte>());
            return await RunSave(() => DeserializeFromStream(new MemoryStream(respBytes), typeof(reCaptcha2ResponseModel)) as reCaptcha2ResponseModel, null);
        }

        /// <summary>
        /// Проверка токена
        /// </summary>
        static byte[] reCaptchaSiteVerify(string secret, string response, string remoteip = null)
        {
            byte[] resp_bytes;
            try
            {
                using (WebClient client = new WebClient() { Encoding = Encoding.UTF8 })
                {
                    NameValueCollection values = new NameValueCollection()
                    {
                       { "secret", secret }, // Требуемый. Общий ключ между вашим сайтом и reCAPTCHA
                       { "response", response } // Требуемый. Маркер ответа пользователя, предоставляемый клиентской интеграцией reCAPTCHA на вашем сайте
                    };
                    if (!string.IsNullOrWhiteSpace(remoteip))
                        values.Add(new NameValueCollection() { { "remoteip", remoteip } });// Необязательный. IP-адрес пользователя

                    resp_bytes = client.UploadValues("https://www.google.com/recaptcha/api/siteverify", values);
                }
            }
            catch
            {
                resp_bytes = new byte[0];
            }
            return resp_bytes;
        }

        /// <summary>
        /// Десереализовать объект из stream
        /// </summary>
        public static object DeserializeFromStream(Stream stream, Type type)
        {
            JsonSerializer serializer = new JsonSerializer();
            try
            {
                using (StreamReader sr = new StreamReader(stream))
                using (JsonTextReader jsonTextReader = new JsonTextReader(sr))
                {
                    return serializer.Deserialize(jsonTextReader, type);
                }
            }
            catch
            {
                return null;
            }
        }

        private static Task RunSave(Action action)
        {
            return Task.Run(() =>
            {
                try
                {
                    action?.Invoke();
                }
                catch (Exception ex)
                {
                    //_logger.Error("Ошибка " + GetType().Name + ex.Message);
                }
            });
        }

        private static Task<T> RunSave<T>(Func<T> func, T def)
        {
            return Task.Run(() =>
            {
                try
                {
                    var res = func.Invoke();
                    return res;
                }
                catch (Exception ex)
                {
                    //_logger.Error("Ошибка " + GetType().Name + ex.Message);
                }

                return def;
            });
        }
    }
}
