using MediatR;
using Proffeo.Models.Exceptions;
using Proffeo.Models.Users;
using Proffeo.Services.Users.Interfaces;
using Proffeo.Services.Users.Utils;

namespace Proffeo.Services.Users.Commands;

internal class CreateUserCommandHandler(IEmailValidator emailValidator, IUserRepository repository) : IRequestHandler<CreateUserCommand, User>
{
    public async Task<User> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        Validate(request);
        var user = User.Create(request.Name, request.Email);
        await repository.AddAsync(user, cancellationToken);
        return user;
    }

    private void Validate(CreateUserCommand request)
    {
        if (string.IsNullOrEmpty(request.Name))
            throw ValidationException.Create<User>("Name", "The name cannot be null or empty.");
        if (!emailValidator.IsValidEmail(request.Email))
            throw ValidationException.Create<User>("Email", "The provided email address is invalid.");
    }
}

public record CreateUserCommand(string Name, string Email) : IRequest<User>;