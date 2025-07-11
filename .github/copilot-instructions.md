<!-- Use this file to provide workspace-specific custom instructions to Copilot. For more details, visit https://code.visualstudio.com/docs/copilot/copilot-customization#_use-a-githubcopilotinstructionsmd-file -->

# サマリ
このワークスペースは、Spectre.Consoleを用いてInk風のリッチなTUIを実現するCLIライブラリ（Blazor.Ink）を構築する.NETソリューションです。コア、CLI、テスト、サンプルプロジェクトを含みます。ターミナルへのコンポーネント描画やユーザー入力への応答を重視し、コアロジックのテストカバレッジを優先します。

# Blazor.Ink Workspace Custom Instructions

## サマリ
Blazor.Inkは、BlazorのレンダーツリーをSpectre.ConsoleベースのTUIに変換するCLIライブラリです。コア（Blazor.Ink.Core）はBlazorコンポーネントをTUIノードツリーへ変換し、YogaSharpでレイアウト計算後、Spectre.Consoleで描画します。

## 設計方針
- RazorファイルはBlazor標準のビルドでC#コンポーネント化し、InkRendererがレンダーツリー→TUIノード（IInkNode）→Spectre.Console描画を担う。
- 各コンポーネント（例: Box, Text）は対応するNode（BoxNode, TextNode）に変換され、YogaSharpでレイアウト。
- レンダーツリー→TUI変換・レイアウト・描画ロジックは小さな単位で分離し、テスト容易性を重視。
- CLIやサンプルはコアロジックを利用してTUIを表示。
- ユーザー入力によるTUIの動的変化も重視。

## コーディング規約
- すべてのC#ファイルはfile-scoped namespace形式（例: `namespace Foo.Bar;`）を原則とする。
- コアロジックのテストカバレッジを優先する。
- unsafeコードの利用は必要最小限とし、明示的に意図をコメントする。
- IDisposable実装クラスはDisposeパターンを厳守する。
- null許容型や`null!`の利用時は意図を明確にし、初期化漏れに注意する。
- partial classの利用は分割実装の意図が明確な場合に限定する。
- XMLドキュメントコメント（///）を積極的に記述し、公開APIの意図・使い方を明示する。
