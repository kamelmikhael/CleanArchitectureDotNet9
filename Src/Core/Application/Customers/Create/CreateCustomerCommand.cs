using Application.Abstractions.Messaging;

namespace Application.Customers.Create;

public record CreateCustomerCommand(string Name, string Email) : ICommand;
