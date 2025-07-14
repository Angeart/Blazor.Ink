using Blazor.Ink.Testing.Internals;
using Spectre.Console;
using Spectre.Console.Rendering;
using Spectre.Console.Testing;

namespace Blazor.Ink.Testing;

public class CursorEmulatedTestConsole : IAnsiConsole, IDisposable
{
    private readonly IAnsiConsole _console;
    private readonly CursorEmulatedWriter _writer;
    private IAnsiConsoleCursor? _cursor;

    /// <inheritdoc/>
    public Profile Profile => _console.Profile;

    /// <inheritdoc/>
    public IExclusivityMode ExclusivityMode => _console.ExclusivityMode;

    /// <summary>
    /// Gets the console input.
    /// </summary>
    public TestConsoleInput Input { get; }

    /// <inheritdoc/>
    public RenderPipeline Pipeline => _console.Pipeline;

    /// <inheritdoc/>
    public IAnsiConsoleCursor Cursor => _cursor ?? _console.Cursor;

    /// <inheritdoc/>
    IAnsiConsoleInput IAnsiConsole.Input => Input;

    /// <summary>
    /// Gets the console output.
    /// </summary>
    public string Output => _writer.ToString();

    /// <summary>
    /// Gets the console output lines.
    /// </summary>
    public IReadOnlyList<string> Lines => Output.NormalizeLineEndings().TrimEnd('\n').Split(new[] { '\n' });

    /// <summary>
    /// Gets or sets a value indicating whether or not VT/ANSI sequences
    /// should be emitted to the console.
    /// </summary>
    public bool EmitAnsiSequences { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TestConsole"/> class.
    /// </summary>
    public CursorEmulatedTestConsole()
    {
        _writer = new CursorEmulatedWriter();
        Input = new TestConsoleInput();
        EmitAnsiSequences = false;

        _console = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Ansi = AnsiSupport.Yes,
            ColorSystem = (ColorSystemSupport)ColorSystem.TrueColor,
            Out = new AnsiConsoleOutput(_writer),
            Interactive = InteractionSupport.No,
            ExclusivityMode = new NoopExclusivityMode(),
            Enrichment = new ProfileEnrichment
            {
                UseDefaultEnrichers = false,
            },
        });
        _cursor = new MoveOnlyCursor(_console);

        _console.Profile.Width = 80;
        _console.Profile.Height = 24;
        _console.Profile.Capabilities.Ansi = true;
        _console.Profile.Capabilities.Unicode = true;
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _writer.Dispose();
    }

    /// <inheritdoc/>
    public void Clear(bool home)
    {
        _console.Clear(home);
    }

    /// <inheritdoc/>
    public void Write(IRenderable renderable)
    {
        if (EmitAnsiSequences)
        {
            _console.Write(renderable);
        }
        else
        {
            foreach (var segment in renderable.GetSegments(this))
            {
                if (segment.IsControlCode)
                {
                    continue;
                }

                Profile.Out.Writer.Write(segment.Text);
            }
        }
    }

    internal void SetCursor(IAnsiConsoleCursor? cursor)
    {
        _cursor = cursor;
    }
}