using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.RoleAggregate;
using NetCorePal.D3Shop.Web.Admin.Client.Attributes;

namespace NetCorePal.D3Shop.Web.Admin.Client.Services;

[RefitService]
public interface IDepartmentService
{
    [Post("/api/Department/CreateDepartment")]
    Task<ResponseData<DeptId>> CreateDepartment([Body] CreateDepartmentRequest request);

    [Get("/api/Department/GetAllDepartments")]
    Task<ResponseData<PagedData<DepartmentResponse>>> GetAllDepartments([Query] DepartmentQueryRequest request);

    [Put("/api/Department/UpdateDepartmentInfo/{id}")]
    Task<ResponseData> UpdateDepartmentInfo(DeptId id, [Body] UpdateDepartmentInfoRequest request);

    [Delete("/api/Department/DeleteDepartment/{id}")]
    Task<ResponseData> DeleteDepartment(DeptId id);


}