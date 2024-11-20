using Microsoft.AspNetCore.Mvc;
using NetCorePal.D3Shop.Web.Auth;

namespace NetCorePal.D3Shop.Web.Controllers;

[Route("test/[controller]")]
[ApiController]
public class AdminUserAuthTestController : ControllerBase
{
    [HttpGet]
    [AdminPermission("AdminUserAuth_Test_Get")]
    public IActionResult Get()
    {
        return Ok("Hello World");
    }

    [HttpPost]
    [AdminPermission("AdminUserAuth_Test_Post")]
    public IActionResult Post()
    {
        return Ok("Hello World");
    }
}