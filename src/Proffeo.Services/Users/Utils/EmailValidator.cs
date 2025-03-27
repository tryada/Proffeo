using System.Text.RegularExpressions;

namespace Proffeo.Services.Users.Utils;

public interface IEmailValidator
{
    bool IsValidEmail(string email);
} 

internal class EmailValidator : IEmailValidator
{
    private static readonly string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

    public bool IsValidEmail(string email)
        => !string.IsNullOrWhiteSpace(email) && Regex.IsMatch(email, EmailPattern);
}