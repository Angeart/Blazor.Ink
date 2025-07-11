using System.Runtime.CompilerServices;
using Blazor.Ink.Test.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Testing;

namespace Blazor.Ink.Test.Components;

public class SnapshotTestBase
{
    private static readonly VerifySettings s_settings = new();

    static SnapshotTestBase()
    {
        s_settings.UseDirectory("Snapshots");
    }
    protected async Task<TestContext> SetupComponent<TComponent>()
        where TComponent : IComponent
    {
        var builder = Ink.CreateBuilder([]);
        var testConsole = new TestConsole();
        var dispatcher = new InkTestingDispatcher();
        builder.ConfigureServices(services =>
        {
            services.AddLogging();
            services.AddSingleton<IAnsiConsole>(
                _ => testConsole);
            services.AddSingleton<InkHost>();
            services.AddSingleton<Dispatcher>(_ => dispatcher);
        });
        var app = builder.Build();
        var cts = new CancellationTokenSource();
        app.RegisterComponents([typeof(TComponent)]);
        _ = app.RunAsync<TComponent>(cts.Token);

        void DisposeAction()
        {
            cts.Dispose();
        }

        // first call to MoveNext is required to render first page
        await dispatcher.MoveNext();

        return new TestContext(
            testConsole,
            dispatcher,
            DisposeAction
        );
    }

    protected SettingsTask VerifyContext(
        TestContext context,
        [CallerFilePath] string filePath = "")
    {
        // ReSharper disable once ExplicitCallerInfoArgument
        return Verify(context.Console.Output, s_settings, filePath);
    }
}