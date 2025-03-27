using MediatR;
using Proffeo.Models.Exceptions;
using Proffeo.Models.Users;
using Proffeo.Services.Users.Interfaces;

namespace Proffeo.Services.Users.Commands;

internal class DeleteUserCommandHandler(IUserRepository repository) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var userToDelete = await repository.GetByIdAsync(request.UserId) 
                           ?? throw NotFoundException.Create<User>("UserId", request.UserId.ToString());
        await repository.DeleteAsync(userToDelete, cancellationToken);
    }
}

public record DeleteUserCommand(Guid UserId) : IRequest;