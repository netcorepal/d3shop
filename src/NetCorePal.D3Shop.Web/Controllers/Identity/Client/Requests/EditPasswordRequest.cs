using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;

namespace NetCorePal.D3Shop.Web.Controllers.Identity.Client.Requests;

public record EditPasswordRequest(
    ClientUserId UserId,
    string OldPassword,
    string NewPassword);