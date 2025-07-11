<!-- Use this file to provide workspace-specific custom instructions to Copilot. For more details, visit https://code.visualstudio.com/docs/copilot/copilot-customization#_use-a-githubcopilotinstructionsmd-file -->

# Summary
This workspace is a .NET solution for building a CLI library (Blazor.Ink) that realizes Ink-style rich TUI using Spectre.Console. It includes core, CLI, test, and sample projects. Emphasis is placed on component rendering to the terminal and responding to user input, with a priority on test coverage for core logic.

# Blazor.Ink Workspace Custom Instructions

## Ignores
- **/bin/**: Ignore all files in the bin directory.
- **/obj/**: Ignore all files in the obj directory.

## Summary
Blazor.Ink is a CLI library that converts Blazor's render tree into a TUI based on Spectre.Console. The core (Blazor.Ink.Core) transforms Blazor components into a TUI node tree, performs layout calculation with YogaSharp, and renders using Spectre.Console.

## Design Principles
- Razor files are compiled into C# components using standard Blazor build, and InkRenderer is responsible for converting the render tree into TUI nodes (IInkNode) and rendering with Spectre.Console.
- Each component (e.g., Box, Text) is converted to a corresponding Node (BoxNode, TextNode) and laid out using YogaSharp.
- The logic for render tree to TUI conversion, layout, and rendering is separated into small units for testability.
- CLI and sample apps use the core logic to display TUIs.
- Dynamic TUI updates in response to user input are emphasized.

## Coding Guidelines
- All C# files should use file-scoped namespace format (e.g., `namespace Foo.Bar;`).
- Prioritize test coverage for core logic.
- Use unsafe code only when necessary, and always comment the intent explicitly.
- Classes implementing IDisposable must strictly follow the Dispose pattern.
- When using nullable types or `null!`, clarify the intent and avoid uninitialized values.
- Use partial classes only when the intent for split implementation is clear.
- Actively write XML documentation comments (///) to clarify the intent and usage of public APIs.
- All comments and documentation written to the codebase must be in English.

## Chat Guidelines
- When responding to user queries, always respect the user's input language.
