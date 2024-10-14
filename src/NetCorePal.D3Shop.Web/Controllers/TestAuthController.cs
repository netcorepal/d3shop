using Microsoft.AspNetCore.Mvc;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.Permission;
using NetCorePal.D3Shop.Web.Attribute;

namespace NetCorePal.D3Shop.Web.Controllers;

[Route("/test/auth")]
[ApiController]
public class TestAuthController : ControllerBase
{
    [HttpGet]
    [MustHavePermission(AppFeature.TestAuth, AppAction.Read)]
    public IActionResult Get()
    {
        return Ok("Hello World");
    }

    [HttpPost]
    [MustHavePermission(AppFeature.TestAuth, AppAction.Create)]
    public IActionResult Post()
    {
        return Ok("Hello World");
    }
}