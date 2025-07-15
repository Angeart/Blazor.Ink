using System.Text;
using System.Text.RegularExpressions;

namespace Blazor.Ink.Testing.Internals;

/// <summary>
/// A TextWriter that emulates cursor movement ANSI escape sequences and reflects output to a virtual buffer (per line StringBuilder).
/// </summary>
internal class CursorEmulatedWriter : TextWriter
{
    private readonly List<StringBuilder> _lines = new();
    private int _cursorLeft = 0;
    private int _cursorTop = 0;

    private static readonly Regex AnsiMoveRegex = new(@"\x1b\[(\d+)?([ABCD])", RegexOptions.Compiled);

    public override Encoding Encoding => Encoding.UTF8;

    public override void Write(char value)
    {
        if (value == '\n')
        {
            _cursorTop++;
            _cursorLeft = 0;
            EnsureLineExists(_cursorTop);
            return;
        }
        if (value == '\r')
        {
            _cursorLeft = 0;
            return;
        }
        EnsureLineExists(_cursorTop);
        var line = _lines[_cursorTop];
        var previousLength = line.Length;
        if (_cursorLeft - previousLength == 1)
        {
            line.Append(value);
        }
        else if (previousLength < _cursorLeft)
        {
            line.Length += _cursorLeft - line.Length + 1;
            for (int i = previousLength; i < line.Length; i++)
            {
                line[i] = ' '; // Fill with spaces to ensure length
            }
            line[_cursorLeft] = value;
        }
        else if (_cursorLeft < previousLength)
        {
            line[_cursorLeft] = value;
        }
        else
        {
            line.Append(value);
        }
        _cursorLeft++;
    }

    public override void Write(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return;

        int lastIndex = 0;
        foreach (Match match in AnsiMoveRegex.Matches(value))
        {
            // Write preceding text
            if (match.Index > lastIndex)
            {
                var text = value.Substring(lastIndex, match.Index - lastIndex);
                foreach (var c in text)
                    Write(c);
            }

            // Handle ANSI move
            int n = 1;
            if (match.Groups[1].Success)
                n = int.Parse(match.Groups[1].Value);

            switch (match.Groups[2].Value)
            {
                case "A": // Up
                    _cursorTop = Math.Max(0, _cursorTop - n);
                    break;
                case "B": // Down
                    _cursorTop += n;
                    EnsureLineExists(_cursorTop);
                    break;
                case "C": // Right
                    _cursorLeft += n;
                    EnsureLineExists(_cursorTop);
                    var line = _lines[_cursorTop];
                    // Fill with spaces if cursor moves beyond current line length
                    while (line.Length < _cursorLeft - 1)
                        line.Append(' ');
                    break;
                case "D": // Left
                    _cursorLeft = Math.Max(0, _cursorLeft - n);
                    break;
            }
            lastIndex = match.Index + match.Length;
        }

        // Write remaining text
        if (lastIndex < value.Length)
        {
            var text = value.Substring(lastIndex);
            foreach (var c in text)
                Write(c);
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (int i = 0; i < _lines.Count; i++)
        {
            sb.Append(_lines[i].ToString());
            if (i < _lines.Count - 1)
                sb.Append(Environment.NewLine);
        }
        return sb.ToString();
    }

    private void EnsureLineExists(int line)
    {
        while (_lines.Count <= line)
            _lines.Add(new StringBuilder());
    }
}