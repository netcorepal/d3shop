namespace NetCorePal.D3Shop.Web.Controllers.Identity.Client.Requests;

public record ClientUserRegisterRequest(
    string NickName,
    string Avatar,
    string Phone,
    string Password,
    string Email);