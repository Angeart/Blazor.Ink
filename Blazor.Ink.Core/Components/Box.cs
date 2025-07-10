using Microsoft.AspNetCore.Components;

namespace Blazor.Ink.Core.Components;

/// <summary>
/// InkのBoxに相当。枠やパディング、マージン等を持つTUI要素。
/// </summary>
public class Box : ComponentBase, IInkComponent
{
    [Parameter]
    public int Padding { get; set; } = 0;
    [Parameter]
    public int Margin { get; set; } = 0;
}
