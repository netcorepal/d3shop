using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.DepartmentAggregate;

namespace NetCorePal.D3Shop.Admin.Shared.Responses;

public class DepartmentResponse(DeptId id, string name, string remark,string code,DeptId parentId,int status,DateTime createTime, List<DepartmentResponse> children)
{
    public DeptId Id { get; } = id;
    public string Name { get; set; } = name;
    public string Remark { get; set; } = remark;
    public string Code { get; set; } = code;
    public DeptId ParentId { get; set; } = parentId;
    public int Status { get; set; } = status;
    public string CreateTime { get; set; } = createTime.ToString("yyyy-MM-dd HH:mm:ss");
    public List<DepartmentResponse> Children { get; set; } = children;

}