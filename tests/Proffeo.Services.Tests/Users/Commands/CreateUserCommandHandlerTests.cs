using Moq;
using Proffeo.Models.Exceptions;
using Proffeo.Models.Users;
using Proffeo.Services.Users.Commands;
using Proffeo.Services.Users.Interfaces;
using Proffeo.Services.Users.Utils;

namespace Proffeo.Services.Tests.Users.Commands;

[TestFixture]
public class CreateUserCommandHandlerTests
{
    private Mock<IUserRepository> _userRepositoryMock;
    private CreateUserCommandHandler _handler;
    private Mock<IEmailValidator> _emailValidator;
    
    [SetUp]
    public void SetUp()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _emailValidator = new Mock<IEmailValidator>();
        _handler = new CreateUserCommandHandler(_emailValidator.Object, _userRepositoryMock.Object);
    }

    [Test]
    public async Task ShouldCreateAndSaveUser()
    {
        var command = CreateUserCommand();
        _emailValidator.Setup(v => v.IsValidEmail(It.IsAny<string>())).Returns(true);
        
        var result = await _handler.Handle(command, CancellationToken.None);

        _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once());
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo(command.Name));
            Assert.That(result.Email, Is.EqualTo(command.Email));
        });
    }

    [Test]
    public async Task ShouldReturnCreatedUser()
    {
        var command = CreateUserCommand();
        _emailValidator.Setup(v => v.IsValidEmail(It.IsAny<string>())).Returns(true);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<User>());
            Assert.That(result.Name, Is.EqualTo(command.Name));
            Assert.That(result.Email, Is.EqualTo(command.Email));
        });
    }

    [Test]
    public void ShouldThrowValidationExceptionWhenUserNameIsInvalid()
    {
        var command = CreateUserCommand("");

        var validationException = Assert.ThrowsAsync<ValidationException>(
            async () => await _handler.Handle(command, CancellationToken.None)
        );
        
        Assert.That(validationException.Message, Is.EqualTo("Validation error in User - field 'Name': The name cannot be null or empty."));
    }

    [Test]
    public void ShouldThrowValidationExceptionWhenUserEmailIsInvalid()
    {
        var command = CreateUserCommand();
        _emailValidator.Setup(v => v.IsValidEmail(It.IsAny<string>())).Returns(false);

        var validationException = Assert.ThrowsAsync<ValidationException>(
            async () => await _handler.Handle(command, CancellationToken.None)
        );
        
        Assert.That(validationException.Message, Is.EqualTo("Validation error in User - field 'Email': The provided email address is invalid."));
    }
    
    private CreateUserCommand CreateUserCommand(string name = "Test User", string email = "test@example.com") 
        => new(name, email);
}