using System.Diagnostics.CodeAnalysis;

namespace NetCorePal.D3Shop.Web.Extensions;

/// <summary>
/// 一些针对 string 的实用扩展方法。
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// 提供对 <see cref="string.IsNullOrWhiteSpace"/> 的简化使用，用于判断字符串是否为 null、空字符串或仅包含空白字符。
    /// </summary>
    /// <param name="str">要检查的字符串。</param>
    /// <returns>
    /// 如果字符串为 null、空字符串或仅包含空白字符，则返回 <c>true</c>；否则返回 <c>false</c>。
    /// </returns>
    public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? str)
    {
        return string.IsNullOrWhiteSpace(str);
    }
}