
using Blazor.Ink.Testing.Internals;

namespace Blazor.Ink.Testing.Tests;

public class CursorEmulatedWriterTests
{
  [Fact]
  public void Write_SimpleText_WritesToBuffer()
  {
    var writer = new CursorEmulatedWriter();
    writer.Write("Hello");
    Assert.Equal("Hello", writer.ToString());
  }

  [Fact]
  public void Write_NewLine_MovesCursorDown()
  {
    var writer = new CursorEmulatedWriter();
    writer.Write("""
                 Hello
                 World
                 """);
    Assert.Equal("""
                 Hello
                 World
                 """, writer.ToString());
  }

  [Fact]
  public void Write_CarriageReturn_MovesCursorToLineStart()
  {
    var writer = new CursorEmulatedWriter();
    writer.Write("Hello\rWorld");
    Assert.Equal("World", writer.ToString());
  }

  [Fact]
  public void Write_AnsiCursorUp_MovesCursorUp()
  {
    var writer = new CursorEmulatedWriter();
    writer.Write("""
                 Line1
                 Line2
                 Line3
                 
                 """);
    writer.Write("\x1b[2ALineX"); // Move up 2 lines and overwrite
    Assert.Equal("""
                 Line1
                 LineX
                 Line3
                 
                 """, writer.ToString());
  }

  [Fact]
  public void Write_AnsiCursorDown_MovesCursorDown()
  {
    var writer = new CursorEmulatedWriter();
    writer.Write("Line1");
    writer.Write("\x1b[2BLine3"); // Move down 2 lines and write
    Assert.Equal("""
                 Line1
                 
                      Line3
                 """, writer.ToString());
  }

  [Fact]
  public void Write_AnsiCursorRight_MovesCursorRight()
  {
    var writer = new CursorEmulatedWriter();
    writer.Write("Hi");
    writer.Write("\x1b[3C!"); // Move right 3 and write
    Assert.Equal("Hi  !", writer.ToString());
  }

  [Fact]
  public void Write_AnsiCursorLeft_MovesCursorLeft()
  {
    var writer = new CursorEmulatedWriter();
    writer.Write("Hello");
    writer.Write("\x1b[3D!"); // Move left 3 and write
    Assert.Equal("He!lo", writer.ToString());
  }

  [Fact]
  public void Write_MixedAnsiAndText_HandlesAll()
  {
    var writer = new CursorEmulatedWriter();
    writer.Write("""
                 A
                 B
                 C
                 """);
    writer.Write("\x1b[2A!\x1b[1B?\x1b[1C*");
    Assert.Equal("""
                 A!
                 B? *
                 C
                 """, writer.ToString());
  }
}
