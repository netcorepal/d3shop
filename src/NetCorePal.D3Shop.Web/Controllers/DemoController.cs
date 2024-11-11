using NetCorePal.D3Shop.Domain.AggregatesModel.OrderAggregate;
using NetCorePal.D3Shop.Web.Application.IntegrationEventHandlers;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NetCorePal.Context;
using NetCorePal.Extensions.DistributedLocks;
using NetCorePal.Extensions.DistributedTransactions;
using NetCorePal.Extensions.Domain;
using NetCorePal.Extensions.Primitives;

namespace NetCorePal.D3Shop.Web.Controllers
{
    [Route("/demo")]
    [ApiController]
    public class DemoController(IMediator mediator)
    {
        [HttpPost]
        [Route("json")]
        public ResponseData<JsonResponse> Json(JsonRequest body)
        {
            return new JsonResponse(body.Id, body.Name, body.Time).AsResponseData();
        }

        [HttpPost]
        [Route("validator")]
        public Task<ResponseData> Validator(ValidatorRequest request)
        {
            ValidatorCommand cmd = new(request.Name, request.Price);
            return mediator.Send(cmd).AsResponseData();
        }

        [HttpPost]
        [Route("event")]
        public async Task<ResponseData<long>> Event([FromServices] IIntegrationEventPublisher publisher)
        {
            await publisher.PublishAsync(new OrderPaidIntegrationEvent(new OrderId(55)));
            return 55L.AsResponseData();
        }

        [HttpPost]
        [Route("context")]
        public ResponseData<string> Context([FromServices] IContextAccessor contextAccessor)
        {
            var tenanContext = contextAccessor.GetContext<TenantContext>();

            return (tenanContext == null ? "" : tenanContext.TenantId).AsResponseData();
        }

        private static bool _isRunning = false;

        [HttpGet]
        [Route("lock")]
        public async Task<ResponseData<bool>> Lock([FromServices] IDistributedLock distributedDisLock)
        {
            if (_isRunning)
            {
                return true.AsResponseData();
            }

            await using var handle = await distributedDisLock.AcquireAsync("lock");
            if (_isRunning)
            {
                return true.AsResponseData();
            }

            _isRunning = true;
            await Task.Delay(1000);
            _isRunning = false;
            return false.AsResponseData();
        }
    }


    public record ValidatorCommand(string Name, int Price) : ICommand;

    public record ValidatorRequest(string Name, int Price);

    public class ValidatorCommandHandler : ICommandHandler<ValidatorCommand>
    {
        public Task Handle(ValidatorCommand request, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

    public class ValidatorCommandValidator : AbstractValidator<ValidatorCommand>
    {
        public ValidatorCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("不能为空").WithErrorCode("code1");
            RuleFor(x => x.Price).InclusiveBetween(18, 60).WithMessage("价格必须在18-60之间").WithErrorCode("code2");
        }
    }

    public partial record My2Id : IInt64StronglyTypedId;

    public partial record MyId : IInt64StronglyTypedId;

    public record JsonRequest(MyId Id, string Name, DateTime Time);

    public record JsonResponse(MyId Id, string Name, DateTime Time);
}