using Proffeo.Models.Tokens;
using Proffeo.Services.Auth.Commands;

namespace Proffeo.Api.Auth;

public record TokenResponse(string Value, DateTime Expires);

internal static class TokenContractsExtensions
{
    public static TokenResponse ToResponse(this Token token) => new(token.Value, token.Expires);

    public static CreateTokenCommand CreateTokenCommand() => new();
}