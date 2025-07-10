<!-- Use this file to provide workspace-specific custom instructions to Copilot. For more details, visit https://code.visualstudio.com/docs/copilot/copilot-customization#_use-a-githubcopilotinstructionsmd-file -->

# サマリ
このワークスペースは、Spectre.Consoleを用いてInk風のリッチなTUIを実現するCLIライブラリ（Blazor.Ink）を構築する.NETソリューションです。コア、CLI、テスト、サンプルプロジェクトを含みます。ターミナルへのコンポーネント描画やユーザー入力への応答を重視し、コアロジックのテストカバレッジを優先します。

# Blazor.Ink Workspace Custom Instructions

## プロジェクト構造
- Blazor.Ink: CLIライブラリ。Spectre.Consoleを用いてTUIをレンダリング。
- Blazor.Ink.Core: Razor/BlazorコンポーネントをTUI表現に変換するコアロジック。
- Blazor.Ink.Test: CLIライブラリのテスト。
- Blazor.Ink.Core.Test: コアロジックのテスト。
- Blazor.Ink.Sample: サンプルアプリ。

## 基本方針
- Razorファイルを直接パース・解釈せず、Blazorの公式エコシステム（Razor→C#コンポーネント→レンダーツリー→Renderer）上でTUI用カスタムRenderer（InkRenderer）としてBlazor.Ink.Coreを組み込む。
- RazorファイルはBlazorのビルドでC#コンポーネント化し、InkRendererはレンダーツリー→TUI変換に専念する。
- RazorファイルやRazor構文のパースには、古いMicrosoft.AspNet.Razorではなく、最新のBlazor/ASP.NET Coreで使われている `Microsoft.AspNetCore.Razor.Language` などのパッケージを利用する。
- 変換ロジックごとにBlazor.Ink.Core.Testでテストを記述し、必ずテストを実行。
- 参考: https://github.com/vadimdemedes/ink (Inkのコンセプトを参考)
- ユーザー入力に応じてTUIのレンダリング結果が変化する仕組みも重視。
- コアロジックのテストカバレッジを優先。

## 生成コードの指針
- Razor構文やコンポーネントの構造をSpectre.ConsoleのAPIで表現できるよう変換ロジックを設計。
- 変換ロジックは小さな単位でテスト可能に実装。
- CLIやサンプルアプリはコアロジックを利用してTUIを表示。

## コーディング規約
- すべてのC#ファイルは、`file-scoped namespace`（例: `namespace Foo.Bar;`）を原則としてください。
- 既存ファイルのnamespaceも順次この形式に統一してください。
