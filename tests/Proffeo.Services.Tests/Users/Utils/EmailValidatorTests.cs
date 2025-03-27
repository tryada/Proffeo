using Proffeo.Services.Users.Utils;

namespace Proffeo.Services.Tests.Users.Utils;

[TestFixture]
public class EmailValidatorTests
{
    private readonly IEmailValidator _emailValidator = new EmailValidator();
    
    [TestCase("user@example.com")]
    [TestCase("test.user@example.co.uk")]
    [TestCase("test123@subdomain.domain.com")]
    [TestCase("name@domain.io")]
    public void ShouldReturnTrueForValidEmail(string email)
    {
        var result = _emailValidator.IsValidEmail(email);

        Assert.That(result, Is.True);
    }

    [TestCase("")]
    [TestCase(" ")]
    [TestCase("\t")]
    [TestCase("test@")]
    [TestCase("@example.com")]
    [TestCase("test.example.com")]
    [TestCase("test@@example.com")]
    [TestCase("test@example")]
    [TestCase("test@.com")]
    [TestCase(" test@example.com")]
    [TestCase("test@example.com ")]
    public void ShouldReturnFalseForInvalidEmail(string email)
    {
        var result = _emailValidator.IsValidEmail(email);

        Assert.That(result, Is.False);
    }
}