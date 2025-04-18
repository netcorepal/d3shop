using NetCorePal.D3Shop.Admin.Shared.Dtos.Identity;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;
using System.ComponentModel.DataAnnotations;

namespace NetCorePal.D3Shop.Admin.Shared.Requests;

public class CreateDepartmentRequest
{

    [Required(ErrorMessage = "部门名称不能为空")]
    public string Name { get; set; } = string.Empty;

    public DeptId Pid { get; set; } = new DeptId(0);

    public int Status { get; set; } = 1;

    public string Remark { get; set; } = string.Empty;

   
}

