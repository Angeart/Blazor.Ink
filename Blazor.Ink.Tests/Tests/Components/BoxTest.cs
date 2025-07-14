using Blazor.Ink.Components;
using Blazor.Ink.Testing;

namespace Blazor.Ink.Tests.Tests.Components;

public class BoxTest : SnapshotTestBase
{
    [Fact]
    public async Task Box_Empty__Snapshot()
    {
        using var context = await SetupComponent<Box>();
        await VerifyContext(context);
    }

    [Fact]
    public async Task Box_DirectText__Snapshot()
    {
        using var context = await SetupComponent<BoxTest_DirectText>();
        await VerifyContext(context);
    }

    [Fact]
    public async Task Box_MarkupText__Snapshot()
    {
        using var context = await SetupComponent<Box>(p =>
            p.AddChildContent("Markup text"));
        await VerifyContext(context);
    }
}