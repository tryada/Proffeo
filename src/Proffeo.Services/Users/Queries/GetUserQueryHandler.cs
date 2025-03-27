using MediatR;
using Proffeo.Models.Exceptions;
using Proffeo.Models.Users;
using Proffeo.Services.Users.Interfaces;

namespace Proffeo.Services.Users.Queries;

internal class GetUserQueryHandler(IUserRepository repository) : IRequestHandler<GetUserQuery, User>
{
    public async Task<User> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetByIdAsync(request.UserId) 
               ?? throw NotFoundException.Create<User>("UserId", request.UserId.ToString());
    }
}

public record GetUserQuery(Guid UserId) : IRequest<User>;