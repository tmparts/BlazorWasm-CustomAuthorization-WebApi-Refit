////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace MetaLib.Services
{
    public abstract class BlazorReCaptchaComponentBaseModel : BlazorBusyComponentBaseModel
    {
        public abstract void OnReCaptchaExpired();

        public abstract void OnReCaptchaSuccess(string response_code);

        public abstract void OnReCaptchaFailure(string response_code);
    }
}
