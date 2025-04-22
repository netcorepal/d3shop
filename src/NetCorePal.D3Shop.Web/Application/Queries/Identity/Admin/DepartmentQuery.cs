using Microsoft.EntityFrameworkCore;
using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Admin.Shared.Responses;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.MenuAggregate;
using NetCorePal.D3Shop.Web.Extensions;
using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Queries.Identity.Admin;

public class DepartmentQuery(ApplicationDbContext applicationDbContext) : IQuery
{
    private DbSet<Department> DepartmentSet { get; } = applicationDbContext.Departments;

    public async Task<bool> DoesDepartmentExist(string name, CancellationToken cancellationToken)
    {
        return await DepartmentSet.AsNoTracking()
            .AnyAsync(r => r.Name == name, cancellationToken: cancellationToken);
    }



    /// <summary>
    /// 获取所有部门
    /// </summary>
    /// <param name="queryRequest"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<List<DepartmentResponse>> GetAllDepartmentsAsync(DepartmentQueryRequest queryRequest, CancellationToken cancellationToken)
    {
        // 查询并构建初始列表
        var departments = await DepartmentSet.AsNoTracking()
            .Where(dt => string.IsNullOrWhiteSpace(queryRequest.Name) || dt.Name.Contains(queryRequest.Name!))
            .Where(dt => dt.IsDeleted == false)
            .OrderBy(dt => dt.Id)
            .Select(d => new DepartmentResponse(
                d.Id,
                d.Name,
                d.Description,
                d.Code,
                d.ParentId,
                d.Status,
                d.CreatedAt,
                new List<DepartmentResponse>()))
            .ToListAsync(cancellationToken);

        // 构建部门树
        var departmentMap = departments.ToDictionary(dept => dept.Id);
        var topLevelDepts = new List<DepartmentResponse>();

        var rootParentId = new DeptId(0);
        foreach (var dept in departments)
        {
            if (dept.ParentId == rootParentId)
            {
                topLevelDepts.Add(dept);
            }
            else if (departmentMap.TryGetValue(dept.ParentId, out var parent))
            {
                parent.Children.Add(dept);
            }
        }
        return topLevelDepts;
    }


    public async Task<DepartmentResponse?> GetDeptByIdAsync(DeptId id, CancellationToken cancellationToken)
    {
        return await DepartmentSet.AsNoTracking()
              .Select(d => new DepartmentResponse(
                d.Id,
                d.Name,
                d.Description,
                d.Code,
                d.ParentId,
                d.Status,
                d.CreatedAt,
                new List<DepartmentResponse>()))
            .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

}