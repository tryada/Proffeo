using Proffeo.Models.Users;

namespace Proffeo.Services.Tests.Users.TestUtils;

public static class UserFixture
{
    public static User User(string name = "Test User", string email = "test@example.com") 
        => Models.Users.User.Create(name, email);
}