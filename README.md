<div align="center">
<img src="./Blazor.Ink/icon.png"/>
</div>

# Blazor.Ink

Blazor.Ink is a .NET library for building rich terminal UIs (TUI) using [Spectre.Console](https://spectreconsole.net/), inspired by [Ink](https://github.com/vadimdemedes/ink).  
It enables you to write Blazor components and render them as terminal applications.

## Solution Structure

- **Blazor.Ink**: CLI library for rendering Blazor components as TUI using Spectre.Console and YogaSharp for layout.
- **Blazor.Ink.Testing**: Utilities for testing, including test consoles and dispatcher mocks.
- **Blazor.Ink.Testing.Tests**: Unit test for testing library.
- **Blazor.Ink.Tests**: Unit and snapshot tests for the CLI library.
- **Blazor.Ink.Sample**: Sample console app demonstrating usage.

## Getting Started

1. Build the solution:
   ```bash
   dotnet build
   ```
2. Run the sample app:
   ```bash
   dotnet run --project Blazor.Ink.Sample
   ```

## Features

- Write Blazor components and render them as terminal UIs.
- Uses YogaSharp for flexbox layout calculation.
- Renders to Spectre.Console for rich TUI output.
- Supports unit and snapshot testing of TUI output.

## Libraries Used

- [Spectre.Console](https://spectreconsole.net/) for TUI rendering
- [YogaSharp](https://github.com/LayoutFarm/YogaSharp) for layout calculation
- [xUnit](https://xunit.net/) and [Verify](https://github.com/VerifyTests/Verify) for testing

## License
MIT
