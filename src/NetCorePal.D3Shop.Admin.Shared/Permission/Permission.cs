using System.Collections.Immutable;

namespace NetCorePal.D3Shop.Admin.Shared.Permission;

/// <summary>
/// 表示一个权限对象，包含权限的基本信息及子权限
/// </summary>
public sealed class Permission
{
    /// <summary>
    /// 权限的唯一名称（代码）
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// 权限的显示名称
    /// </summary>
    public string DisplayName { get; }

    /// <summary>
    /// 当前权限的所有子权限
    /// 子权限是只读的
    /// </summary>
    public IReadOnlyList<Permission> Children => _children.ToImmutableList();

    private readonly List<Permission> _children;

    /// <summary>
    /// 指示当前权限是否启用
    /// 默认情况下权限是启用的
    /// 禁用的权限无法被授予，但仍然可以检查其值（始终为 false）
    /// 禁用权限可以用来隐藏相关的应用功能
    /// 默认值：true（启用）
    /// </summary>
    public bool IsEnabled { get; }

    /// <summary>
    /// 创建一个新的权限对象。
    /// </summary>
    /// <param name="code">权限的唯一代码。</param>
    /// <param name="displayName">权限的显示名称。</param>
    /// <param name="isEnabled">是否启用此权限，默认为 true。</param>
    internal Permission(
        string code,
        string displayName,
        bool isEnabled = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(displayName);

        Code = code;
        DisplayName = displayName;
        IsEnabled = isEnabled;
        _children = [];
    }

    /// <summary>
    /// 向当前权限添加一个子权限。
    /// </summary>
    /// <param name="code">子权限的唯一代码。</param>
    /// <param name="displayName">子权限的显示名称。</param>
    /// <param name="isEnabled">子权限是否启用，默认为 true。</param>
    /// <returns>返回创建的子权限对象。</returns>
    public Permission AddChild(string code, string displayName, bool isEnabled = true)
    {
        var child = new Permission(code, displayName, isEnabled);

        _children.Add(child);

        return child;
    }
}