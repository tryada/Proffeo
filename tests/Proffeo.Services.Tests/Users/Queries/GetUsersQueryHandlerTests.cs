using Moq;
using Proffeo.Models.Users;
using Proffeo.Services.Tests.Users.TestUtils;
using Proffeo.Services.Users.Interfaces;
using Proffeo.Services.Users.Queries;

namespace Proffeo.Services.Tests.Users.Queries;

[TestFixture]
public class GetUsersQueryHandlerTests
{
    private Mock<IUserRepository> _userRepositoryMock;
    private GetUsersQueryHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _handler = new GetUsersQueryHandler(_userRepositoryMock.Object);
    }

    [Test]
    public async Task ShouldReturnListOfUsersWhenUsersExist()
    {
        var users = new List<User>
        {
            UserFixture.User("User1", "user1@example.com"),
            UserFixture.User("User2", "user2@example.com"),
            UserFixture.User("User3", "user3@example.com")
        };
        _userRepositoryMock
            .Setup(repo => repo.GetUsersPaged(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(users);
        var query = GetAllUsersQuery();

        var result = await _handler.Handle(query, CancellationToken.None);

        _userRepositoryMock.Verify(repo => repo.GetUsersPaged(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        Assert.That(result.Count, Is.EqualTo(users.Count));
        for (var i = 0; i < result.Count; i++)
        {
            Assert.That(result[i].Id, Is.EqualTo(users[i].Id));
            Assert.That(result[i].Name, Is.EqualTo(users[i].Name));
            Assert.That(result[i].Email, Is.EqualTo(users[i].Email));
        }
    }

    [Test]
    public async Task ShouldReturnEmptyListWhenNoUsersExist()
    {
        _userRepositoryMock
            .Setup(repo => repo.GetUsersPaged(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync([]);
        var query = GetAllUsersQuery();

        var result = await _handler.Handle(query, CancellationToken.None);

        _userRepositoryMock.Verify(repo => repo.GetUsersPaged(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
        Assert.That(result, Is.Empty);
    }
    
    private GetUsersQuery GetAllUsersQuery() => new(1, 10);
}