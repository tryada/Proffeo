using Moq;
using Proffeo.Models.Exceptions;
using Proffeo.Models.Users;
using Proffeo.Services.Tests.Users.TestUtils;
using Proffeo.Services.Users.Commands;
using Proffeo.Services.Users.Interfaces;
using Proffeo.Services.Users.Utils;

namespace Proffeo.Services.Tests.Users.Commands;

[TestFixture]
public class UpdateUserCommandHandlerTests
{
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IEmailValidator> _emailValidator;
    private UpdateUserCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userRepositoryMock
            .Setup(repo => repo.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _emailValidator = new Mock<IEmailValidator>();
        _handler = new UpdateUserCommandHandler(_emailValidator.Object, _userRepositoryMock.Object);
    }

    [Test]
    public async Task ShouldUpdateAndSaveUser()
    {
        var existingUser = UserFixture.User();
        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(existingUser.Id))
            .ReturnsAsync(existingUser);
        _emailValidator.Setup(v => v.IsValidEmail(It.IsAny<string>())).Returns(true);
        var command = UpdateUserCommand(existingUser.Id);

        var result = await _handler.Handle(command, CancellationToken.None);

        _userRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once());
        Assert.Multiple(() =>
        {
            Assert.That(result.Name, Is.EqualTo(command.Name));
            Assert.That(result.Email, Is.EqualTo(command.Email));
            Assert.That(result.Id, Is.EqualTo(existingUser.Id));
        });
    }

    [Test]
    public async Task ShouldReturnUpdatedUser()
    {
        var existingUser = UserFixture.User();
        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(existingUser.Id))
            .ReturnsAsync(existingUser);
        _emailValidator.Setup(v => v.IsValidEmail(It.IsAny<string>())).Returns(true);
        var command = UpdateUserCommand(existingUser.Id);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.Multiple(() =>
        {
            Assert.That(result.Name, Is.EqualTo(command.Name));
            Assert.That(result.Email, Is.EqualTo(command.Email));
            Assert.That(result.Id, Is.EqualTo(existingUser.Id));
        });
    }

    [Test]
    public void ShouldThrowNotFoundExceptionWhenUserDoesNotExist()
    {
        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((User?)null);
        var command = UpdateUserCommand();

        var notFoundException = Assert.ThrowsAsync<NotFoundException>(
            async () => await _handler.Handle(command, CancellationToken.None)
        );
        
        Assert.That(notFoundException.Message, Is.EqualTo($"Resource of type User not found for UserId = {command.UserId}"));
    }

    [Test]
    public void ShouldThrowValidationExceptionWhenUserNameIsInvalid()
    {
        var existingUser = UserFixture.User();
        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(existingUser.Id))
            .ReturnsAsync(existingUser);
        var command = UpdateUserCommand(existingUser.Id, "");

        var validationException = Assert.ThrowsAsync<ValidationException>(
            async () => await _handler.Handle(command, CancellationToken.None)
        );
        
        Assert.That(validationException.Message, Is.EqualTo("Validation error in User - field 'Name': The name cannot be null or empty."));
    }

    [Test]
    public void ShouldThrowValidationExceptionWhenUserEmailIsInvalid()
    {
        var userId = Guid.NewGuid();
        var existingUser = UserFixture.User();
        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync(existingUser);
         _emailValidator.Setup(v => v.IsValidEmail(It.IsAny<string>())).Returns(false);
        var command = UpdateUserCommand(userId);

        var validationException = Assert.ThrowsAsync<ValidationException>(
            async () => await _handler.Handle(command, CancellationToken.None)
        );
        Assert.That(validationException.Message, Is.EqualTo("Validation error in User - field 'Email': The provided email address is invalid."));
    }

    private UpdateUserCommand UpdateUserCommand(Guid? userId = null, string name = "New Name", string email = "new@example.com") 
        => new(userId ?? Guid.NewGuid(), name, email);
}