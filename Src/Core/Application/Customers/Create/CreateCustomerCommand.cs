using Application.Abstractions.Messaging;
using Domain.Customers;
using FluentValidation;

namespace Application.Customers.Create;

public record CreateCustomerCommand(string Name, string Email) : ICommand;

internal sealed class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator(ICustomerRepository repository)
    {
        RuleFor(c => c.Email)
            .MustAsync(async (email, _) => await repository.IsEmailUniqueAsync(email, default))
            .WithMessage("Email is already used");
    }
}
