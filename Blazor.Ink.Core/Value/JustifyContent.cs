// JustifyContent.cs
// Inkのstyles.tsを参考にした主軸方向の揃えのenum定義

namespace Blazor.Ink.Core.Value;

/// <summary>
/// 主軸方向の揃え
/// </summary>
public enum JustifyContent
{
    FlexStart,
    FlexEnd,
    SpaceBetween,
    SpaceAround,
    SpaceEvenly,
    Center
}
