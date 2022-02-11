////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace LibMetaApp.Models
{
    public class ClientConfigModel : BaseConfigModel
    {
        public static explicit operator ClientConfigModel(ServerConfigModel v)
        {
            ClientConfigModel res = new ClientConfigModel()
            {
                ApiConfig = v.ApiConfig,
                ClientConfig = v.ClientConfig,
                ReCaptchaConfig = v.ReCaptchaConfig,
            };

            return res;
        }
    }
}
