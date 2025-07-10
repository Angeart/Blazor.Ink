# Blazor.Ink

Blazor.Ink is a .NET solution for building rich terminal UIs (TUI) using Spectre.Console, inspired by Ink. It is composed of:

- **Blazor.Ink**: CLI library for rendering TUI components
- **Blazor.Ink.Core**: Core logic and rendering engine
- **Blazor.Ink.Test**: Tests for the CLI library
- **Blazor.Ink.Core.Test**: Tests for the core logic
- **Blazor.Ink.Sample**: Sample console app demonstrating usage

## Getting Started

1. Build the solution:
   ```bash
   dotnet build
   ```
2. Run the sample app:
   ```bash
   dotnet run --project Blazor.Ink.Sample
   ```

## Libraries Used
- [Spectre.Console](https://spectreconsole.net/) for TUI rendering

## Project Structure
- `Blazor.Ink/` - CLI library
- `Blazor.Ink.Core/` - Core logic
- `Blazor.Ink.Test/` - CLI tests
- `Blazor.Ink.Core.Test/` - Core tests
- `Blazor.Ink.Sample/` - Sample app

## License
MIT
