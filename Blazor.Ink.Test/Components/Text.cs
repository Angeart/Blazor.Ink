using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Blazor.Ink.Test.Components;

public sealed class Text : SnapshotTestBase
{
    public class TextComponentContainer : ComponentBase
    {
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenComponent<Blazor.Ink.Components.Text>(0);
            builder.AddAttribute(1, "ChildContent",
                (RenderFragment)(contentBuilder => { contentBuilder.AddContent(2, "Hello, World!"); }));
            builder.CloseComponent();
        }
    }
    
    [Fact]
    public async Task RenderText__Snapshot()
    {
        var context = await SetupComponent<TextComponentContainer>();
        await VerifyContext(context);
        context.Dispose();
    }
}