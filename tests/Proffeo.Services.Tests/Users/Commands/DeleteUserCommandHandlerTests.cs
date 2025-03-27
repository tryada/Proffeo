using Moq;
using Proffeo.Models.Exceptions;
using Proffeo.Models.Users;
using Proffeo.Services.Tests.Users.TestUtils;
using Proffeo.Services.Users.Commands;
using Proffeo.Services.Users.Interfaces;

namespace Proffeo.Services.Tests.Users.Commands;

[TestFixture]
public class DeleteUserCommandHandlerTests
{
    private Mock<IUserRepository> _userRepositoryMock;
    private DeleteUserCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new DeleteUserCommandHandler(_userRepositoryMock.Object);
    }

    [Test]
    public async Task ShouldDeleteUserWhenUserExists()
    {
        var existingUser = UserFixture.User();
        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(existingUser.Id))
            .ReturnsAsync(existingUser);
        _userRepositoryMock
            .Setup(repo => repo.DeleteAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        var command = DeleteUserCommand(existingUser.Id);

        await _handler.Handle(command, CancellationToken.None);

        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(existingUser.Id), Times.Once());
        _userRepositoryMock.Verify(repo => repo.DeleteAsync(existingUser, It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public void ShouldThrowNotFoundExceptionWhenUserDoesNotExist()
    {
        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((User?)null);
        var command = DeleteUserCommand();

        var notFoundException = Assert.ThrowsAsync<NotFoundException>(
            async () => await _handler.Handle(command, CancellationToken.None)
        );
        
        Assert.That(notFoundException.Message, Is.EqualTo($"Resource of type User not found for UserId = {command.UserId}"));
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(It.IsAny<Guid>()), Times.Once());
        _userRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never());
    }
    
    private DeleteUserCommand DeleteUserCommand(Guid? userId = null) => new(userId ?? Guid.NewGuid());
}