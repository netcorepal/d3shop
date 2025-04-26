using Microsoft.EntityFrameworkCore;
using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Admin.Shared.Responses;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using NetCorePal.D3Shop.Web.Extensions;
using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;

public class UserDeptQuery(ApplicationDbContext applicationDbContext) : IQuery
{
    private DbSet<UserDept> UserDeptSet { get; } = applicationDbContext.UserDepts;


    /// <summary>
    ///  获取部门下的用户数量
    /// </summary>
    /// <param name="deptId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<int> GetUserCount(DeptId deptId, CancellationToken cancellationToken)
    {
        // 查询并构建初始列表
        var userCount = await UserDeptSet.AsNoTracking()
            .Where(d => d.DeptId == deptId)
            .CountAsync();
        return userCount;
    }
}