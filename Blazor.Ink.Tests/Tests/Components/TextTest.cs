using Blazor.Ink.Testing;

namespace Blazor.Ink.Tests.Tests.Components;

public sealed class TextTest : SnapshotTestBase
{
    [Fact]
    public async Task Text_Simple__Snapshot()
    {
        var context = await SetupComponent<TextTest_Simple>();
        await VerifyContext(context);
        context.Dispose();
    }
}