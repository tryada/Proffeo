using Moq;
using Proffeo.Models.Exceptions;
using Proffeo.Models.Users;
using Proffeo.Services.Tests.Users.TestUtils;
using Proffeo.Services.Users.Interfaces;
using Proffeo.Services.Users.Queries;

namespace Proffeo.Services.Tests.Users.Queries;

[TestFixture]
public class GetUserQueryHandlerTests
{
    private Mock<IUserRepository> _userRepositoryMock;
    private GetUserQueryHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new GetUserQueryHandler(_userRepositoryMock.Object);
    }

    [Test]
    public async Task ShouldReturnUserWhenUserExists()
    {
        var expectedUser = UserFixture.User();
        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(expectedUser.Id))
            .ReturnsAsync(expectedUser);
        var query = GetUserQuery(expectedUser.Id);

        var result = await _handler.Handle(query, CancellationToken.None);

        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(expectedUser.Id), Times.Once());
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(expectedUser.Id));
            Assert.That(result.Name, Is.EqualTo(expectedUser.Name));
            Assert.That(result.Email, Is.EqualTo(expectedUser.Email));
        });
    }

    [Test]
    public void ShouldThrowNotFoundExceptionWhenUserDoesNotExist()
    {
        var userId = Guid.NewGuid();
        _userRepositoryMock
            .Setup(repo => repo.GetByIdAsync(userId))
            .ReturnsAsync((User?)null);
        var query = GetUserQuery(userId);

        var notFoundException = Assert.ThrowsAsync<NotFoundException>(
            async () => await _handler.Handle(query, CancellationToken.None)
        );
        Assert.That(notFoundException.Message, Is.EqualTo($"Resource of type User not found for UserId = {query.UserId}"));
        _userRepositoryMock.Verify(repo => repo.GetByIdAsync(userId), Times.Once());
    }
    
    private GetUserQuery GetUserQuery(Guid userId) => new(userId);    
}