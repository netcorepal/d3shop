using Microsoft.AspNetCore.Mvc;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.Client.Requests;

public record ClientUserLoginRequest(
    string Phone,
    string Password,
    string LoginMethod,
    [FromHeader(Name = "User-Agent")] string UserAgent
);