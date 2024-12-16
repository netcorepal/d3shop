using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NetCorePal.D3Shop.Admin.Shared.Requests;
using NetCorePal.D3Shop.Admin.Shared.Responses;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.AdminUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Web.Const;
using NetCorePal.D3Shop.Web.Controllers.Identity.Dto;
using NetCorePal.D3Shop.Web.Extensions;
using NetCorePal.Extensions.Dto;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Queries.Identity;

public class DepartmentQuery(ApplicationDbContext applicationDbContext, IMemoryCache memoryCache) : IQuery
{
    private DbSet<Department> DepartmentSet { get; } = applicationDbContext.Departments;

    public async Task<bool> DoesDepartmentExist(string name, CancellationToken cancellationToken)
    {
        return await DepartmentSet.AsNoTracking()
            .AnyAsync(r => r.Name == name, cancellationToken: cancellationToken);
    }


    public async Task<PagedData<DepartmentResponse>> GetAllDepartmentsAsync(DepartmentQueryRequest queryRequest,
        CancellationToken cancellationToken)
    {
        var departments = await DepartmentSet.AsNoTracking()
            .WhereIf(!queryRequest.Name.IsNullOrWhiteSpace(), dt => dt.Name.Contains(queryRequest.Name!))
            .OrderBy(dt => dt.Id)
            .Select(dt => new DepartmentResponse(dt.Id, dt.Name, dt.Description))
            .ToPagedDataAsync(queryRequest, cancellationToken);
        return departments;
    }

  
}