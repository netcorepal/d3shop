using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.Permission;
using NetCorePal.D3Shop.Web.Admin.Client.Attributes;

namespace NetCorePal.D3Shop.Web.Admin.Client.Services;

[RefitService]
public interface IPermissionsService
{
    [Get("/api/Permission/GetAll")]
    Task<ResponseData<IEnumerable<Permission>>> GetAll();
}