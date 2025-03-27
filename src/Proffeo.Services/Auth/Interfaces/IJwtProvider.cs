namespace Proffeo.Services.Auth.Interfaces;

public interface IJwtProvider
{
    (string, DateTime) GenerateToken();
}