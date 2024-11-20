using Microsoft.AspNetCore.Mvc;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.Permission;
using NetCorePal.Extensions.Dto;

namespace NetCorePal.D3Shop.Web.Controllers.Identity;

[Route("api/[controller]/[action]")]
[ApiController]
public class PermissionController
{
    [HttpGet]
    public Task<ResponseData<IEnumerable<Permission>>> GetAll()
    {
        IEnumerable<Permission> permissions = Permissions.AllPermissions;

        return Task.FromResult(permissions.AsResponseData());
    }
}