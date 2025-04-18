using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;

namespace NetCorePal.D3Shop.Admin.Shared.Requests;

/// <summary>
/// 添加用户到部门请求
/// </summary>
public class AddUserDeptRequest
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public AdminUserId AdminUserId { get; set; } = default!;

    /// <summary>
    /// 部门ID
    /// </summary>
    public DeptId DeptId { get; set; } = default!;

    /// <summary>
    /// 部门名称
    /// </summary>
    public string DeptName { get; set; } = string.Empty;
} 