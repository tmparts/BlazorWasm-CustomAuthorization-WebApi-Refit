////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models.enums;

namespace MetaLib.Models
{
    public class ReCaptchaConfigClientModel
    {
        public ReCaptchaModesEnum Mode { get; set; } = ReCaptchaModesEnum.None;

        public KeysPairsConfigClientModel ReCaptchaV2Config { get; set; } = new KeysPairsConfigClientModel();

        public KeysPairsConfigClientModel ReCaptchaV2InvisibleConfig { get; set; } = new KeysPairsConfigClientModel();

        /*public KeysPairsConfigClientModel ReCaptchaV3Config { get; set; } = new();*/
    }
}