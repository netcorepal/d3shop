using FluentValidation;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserAggregate;
using NetCorePal.D3Shop.Domain.AggregatesModel.Identity.ClientUserLoginHistoryAggregate;
using NetCorePal.D3Shop.Infrastructure.Repositories.Identity.Client;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Application.Commands.Identity.Client;

public record RecordClientUserLoginCommand(
    ClientUserId UserId,
    string NickName,
    DateTimeOffset LoginTime,
    string LoginMethod,
    string IpAddress,
    string UserAgent) : ICommand;

public class RecordClientUserLoginCommandValidator : AbstractValidator<RecordClientUserLoginCommand>
{
}

public class RecordClientUserLoginCommandHandler(ClientUserLoginHistoryRepository clientUserLoginHistoryRepository)
    : ICommandHandler<RecordClientUserLoginCommand>
{
    public async Task Handle(RecordClientUserLoginCommand request, CancellationToken cancellationToken)
    {
        var userLoginHistory = new ClientUserLoginHistory(request.UserId, request.NickName, request.LoginTime,
            request.LoginMethod, request.IpAddress, request.UserAgent);
        await clientUserLoginHistoryRepository.AddAsync(userLoginHistory, cancellationToken);
    }
}