using Domain.Customers;
using FluentValidation;

namespace Application.Customers.Create;

internal sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator(ICustomerRepository repository)
    {
        RuleFor(c => c.Email)
            .MustAsync(async (email, _) => await repository.IsEmailUniqueAsync(email, default))
            .WithMessage("Email is already used");
    }
}
