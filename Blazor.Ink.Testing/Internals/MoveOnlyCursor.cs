using Spectre.Console;
using static Blazor.Ink.Testing.Internals.AnsiSequences;

namespace Blazor.Ink.Testing.Internals;

internal sealed class MoveOnlyCursor : IAnsiConsoleCursor
{
    private readonly IAnsiConsole _console;

    public MoveOnlyCursor(IAnsiConsole console)
    {
        _console = console;
    }

    public void Move(CursorDirection direction, int steps)
    {
        if (steps == 0)
        {
            return;
        }

        switch (direction)
        {
            case CursorDirection.Up:
                _console.Write(new ControlCode(CUU(steps)));
                break;
            case CursorDirection.Down:
                _console.Write(new ControlCode(CUD(steps)));
                break;
            case CursorDirection.Right:
                _console.Write(new ControlCode(CUF(steps)));
                break;
            case CursorDirection.Left:
                _console.Write(new ControlCode(CUB(steps)));
                break;
        }
    }

    public void SetPosition(int column, int line)
    {
    }

    public void Show(bool show)
    {
    }
}