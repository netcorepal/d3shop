using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;

namespace NetCorePal.D3Shop.Web.Application.Queries.Identity.Client.Dto;

public record ClientUserInfo(
    ClientUserId UserId,
    string NickName,
    string Avatar,
    string Phone,
    string Email);