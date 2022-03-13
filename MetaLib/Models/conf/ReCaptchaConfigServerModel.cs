////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace LibMetaApp.Models
{
    public class ReCaptchaConfigServerModel : ReCaptchaConfigClientModel
    {
        public new KeysPairsConfigServerModel ReCaptchaV2Config { get; set; } = new KeysPairsConfigServerModel();

        public new KeysPairsConfigServerModel ReCaptchaV2InvisibleConfig { get; set; } = new KeysPairsConfigServerModel();

        /*public KeysPairsConfigServerModel ReCaptchaV3Config { get; set; } = new KeysPairsConfigModel();*/
    }
}