////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

using MetaLib.Models.enums;
using Microsoft.AspNetCore.Components;

namespace MetaLib.Services
{
    public abstract class BlazorBusyComponentBaseModel : ComponentBase
    {
        public bool IsBusyProgress { get; protected set; } = false;

        public string PrimaryButtonClass(BootstrapColorsStylesEnum ColorStyle) => $"btn text-{ColorStyle.ToString().ToLower()}{(IsBusyProgress ? " disabled" : string.Empty)}";
        public string PrimaryButtonStyle => $"padding:0;margin:0;text-decoration:{(IsBusyProgress ? "none" : "underline")};cursor:{(IsBusyProgress ? "none" : "pointer")};";
    }
}
