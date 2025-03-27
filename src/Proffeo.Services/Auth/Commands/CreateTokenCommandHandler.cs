using MediatR;
using Proffeo.Models.Tokens;
using Proffeo.Services.Auth.Interfaces;

namespace Proffeo.Services.Auth.Commands;

public class CreateTokenCommandHandler(IJwtProvider jwtProvider) : IRequestHandler<CreateTokenCommand, Token>
{
    public Task<Token> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
    {
        var (jwtToken, expires) = jwtProvider.GenerateToken();
        return Task.FromResult(Token.Create(jwtToken, expires));
    }
}

public record CreateTokenCommand : IRequest<Token>;