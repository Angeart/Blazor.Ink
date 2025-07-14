using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace Blazor.Ink.Core;

public static class MarkupStringExtension
{
    public static RenderFragment ToMarkupRenderFragment([StringSyntax("html")] this string? markup)
    {
        if (string.IsNullOrEmpty(markup))
        {
            return _ => { };
        }

        return builder =>
        {
            builder.AddMarkupContent(0, markup);
        };
    }
}