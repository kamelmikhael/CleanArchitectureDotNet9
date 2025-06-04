using Application.Abstractions.Authentication;
using SharedKernal.Abstractions.Data;
using Application.Users.Register;
using Domain.Users;
using FluentAssertions;
using Moq;
using SharedKernal.Abstraction.EventBus;

namespace Application.UnitTests.Users.Register;

public class RegisterUserCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IEventBusService> _eventBusServiceMock;

    private readonly RegisterUserCommandHandler _handler;

    public RegisterUserCommandHandlerTests()
    {
        _userRepositoryMock = new();
        _unitOfWorkMock = new();
        _passwordHasherMock = new();
        _eventBusServiceMock = new();

        _handler = new RegisterUserCommandHandler(
            _unitOfWorkMock.Object,
            _userRepositoryMock.Object,
            _passwordHasherMock.Object,
            _eventBusServiceMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailureResult_WhenEmailIsNotUnique()
    {
        //Arrange
        var command = new RegisterUserCommand("username", "email@gmail.com", "password");

        var emailResult = Email.Create(command.Email);

        _userRepositoryMock
            .Setup(repo => 
                repo.IsEmailExistsAsync(emailResult.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsFailure.Should().BeTrue();
        result.Errors.Should().Contain(UserErrors.EmailNotUnique);
        _userRepositoryMock.Verify(
            repo => repo.Add(It.IsAny<User>()), 
            Times.Never);
        _unitOfWorkMock.Verify(
            uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), 
            Times.Never);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessResult_WhenEmailIsUnique()
    {
        //Arrange
        var command = new RegisterUserCommand("username", "email@gmail.com", "password");

        var emailResult = Email.Create(command.Email);

        _userRepositoryMock
            .Setup(repo =>
                repo.IsEmailExistsAsync(emailResult.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        _userRepositoryMock.Verify(
            repo => repo.Add(It.Is<User>(u => u.Id == result.Value)), 
            Times.Once);
        _unitOfWorkMock.Verify(
            uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), 
            Times.Once);
    }
}
