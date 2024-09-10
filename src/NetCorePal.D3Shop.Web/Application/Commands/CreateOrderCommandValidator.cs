using FluentValidation;

namespace NetCorePal.D3Shop.Web.Application.Commands
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(10).WithErrorCode("name error code");
            RuleFor(x => x.Price).InclusiveBetween(18, 60).WithErrorCode("price error code");
        }
    }
}
