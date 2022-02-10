using Microsoft.AspNetCore.Components;

namespace WebMetaApp.UIInterfaces
{
    public interface ITab
    {
        RenderFragment ChildContent { get; }
    }
}
