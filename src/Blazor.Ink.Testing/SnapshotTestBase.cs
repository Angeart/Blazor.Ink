using System.Runtime.CompilerServices;
using Blazor.Ink.Core;
using Blazor.Ink.Testing.Internals;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Testing;

namespace Blazor.Ink.Testing;

public class SnapshotTestBase
{
    private static readonly VerifySettings s_settings = new();

    static SnapshotTestBase()
    {
        s_settings.UseDirectory("Snapshots");
    }
    protected async Task<TestContext> SetupComponent<TComponent>(ComponentParametersBuilder<TComponent>.BuilderFunction? parametersBuilder = null)
        where TComponent : IComponent
    {
        var builder = Ink.CreateBuilder([]);
        var testConsole = new CursorEmulatedTestConsole();
        testConsole.EmitAnsiSequences = true;
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
        _ = app.RunAsync(cts.Token);
        if (parametersBuilder is not null)
        {
            _ = app.Navigate(parametersBuilder);
        }
        else
        {
            _ = app.Navigate<TComponent>();
        }

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