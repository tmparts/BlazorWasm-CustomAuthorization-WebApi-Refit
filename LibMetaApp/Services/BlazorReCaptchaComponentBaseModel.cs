////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using Microsoft.AspNetCore.Components;

namespace LibMetaApp.Services
{
    public abstract class BlazorReCaptchaComponentBaseModel : ComponentBase
    {
        protected bool InRestProgress { get; set; } = false;

        public abstract void OnReCaptchaExpired();

        public abstract void OnReCaptchaSuccess(string response_code);

        public abstract void OnReCaptchaFailure(string response_code);
    }
}
