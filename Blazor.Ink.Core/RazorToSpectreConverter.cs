// using Spectre.Console;
// using Spectre.Console.Rendering;

// namespace Blazor.Ink.Core;

// /// <summary>
// /// RazorElementをSpectre.ConsoleのRenderableに変換する変換器。
// /// </summary>
// public static class RazorToSpectreConverter
// {
//     public static IRenderable Convert(RazorElement element)
//     {
//         switch (element)
//         {
//             case RazorText t:
//                 return new Markup(Escape(t.Text));
//             case RazorStyledText st:
//                 var markup = Escape(st.Text);
//                 if (st.Bold) markup = "[bold]" + markup + "[/]";
//                 if (st.Italic) markup = "[italic]" + markup + "[/]";
//                 if (st.Color.HasValue) markup = $"[{st.Color.Value.ToString().ToLower()}]" + markup + "[/]";
//                 return new Markup(markup);
//             case RazorNewline:
//                 return new Markup("\n");
//             case RazorElementGroup g:
//                 return new Rows(g.Children.Select(Convert));
//             case RazorFragment f:
//                 return new Rows(f.Children.Select(Convert));
//             case Box b:
//                 var child = Convert(b.Child);
//                 var panel = new Panel(child)
//                 {
//                     Padding = new Padding(b.Padding, b.Padding),
//                     // Spectre.ConsoleのPanelはMargin未対応なので省略
//                 };
//                 return panel;
//             default:
//                 return new Markup("");
//         }
//     }
//     private static string Escape(string text) => Markup.Escape(text);
// }
