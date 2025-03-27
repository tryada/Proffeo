using MediatR;
using Proffeo.Models.Exceptions;
using Proffeo.Models.Users;
using Proffeo.Services.Users.Interfaces;
using Proffeo.Services.Users.Utils;

namespace Proffeo.Services.Users.Commands;

internal class UpdateUserCommandHandler(IEmailValidator emailValidator, IUserRepository repository) : IRequestHandler<UpdateUserCommand, User>
{
    public async Task<User> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await repository.GetByIdAsync(request.UserId) 
                   ?? throw NotFoundException.Create<User>("UserId", request.UserId.ToString());
        Validate(request);
        user.UpdateName(request.Name);
        user.UpdateEmail(request.Email);
        await repository.UpdateAsync(user, cancellationToken);
        return user;
    }

    private void Validate(UpdateUserCommand request)
    {
        if (string.IsNullOrEmpty(request.Name))
            throw ValidationException.Create<User>("Name", "The name cannot be null or empty.");
        if (!emailValidator.IsValidEmail(request.Email))
            throw ValidationException.Create<User>("Email", "The provided email address is invalid.");
    }
}

public record UpdateUserCommand(Guid UserId, string Name, string Email) : IRequest<User>;