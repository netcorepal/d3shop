using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.Permission;

namespace NetCorePal.D3Shop.Web.Admin.Client.Services;

public interface IPermissionsService
{
    [Get("/api/Permission/GetAll")]
    Task<ResponseData<IEnumerable<Permission>>> GetAll();
}