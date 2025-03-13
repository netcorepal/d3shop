using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;

namespace NetCorePal.D3Shop.Admin.Shared.Dtos.Identity
{

    /// <summary>
    /// 创建部门用户信息
    /// </summary>
    /// <param name="UserId"></param>
    /// <param name="UserName"></param>
    public record CreateDepartmentUserInfoDto(AdminUserId UserId, string UserName);
}
