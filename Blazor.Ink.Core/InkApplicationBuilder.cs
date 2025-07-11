using Microsoft.Extensions.DependencyInjection;

namespace Blazor.Ink.Core;

public class InkApplicationBuilder
{
    public IServiceCollection Services { get; }
    public string[] Args { get; }

    public InkApplicationBuilder(string[] args)
    {
        Args = args;
        Services = new ServiceCollection();
        Services.AddLogging(); // Added
    }

    public InkApplication Build()
    {
        return new InkApplication(Services.BuildServiceProvider(), Args);
    }
}
