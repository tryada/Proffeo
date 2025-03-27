using MediatR;
using Proffeo.Models.Users;
using Proffeo.Services.Users.Interfaces;

namespace Proffeo.Services.Users.Queries;

internal class GetUsersQueryHandler(IUserRepository repository) : IRequestHandler<GetUsersQuery, List<User>>
{
    public async Task<List<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var skip = (request.Page - 1) * request.PageSize;
        var take = request.PageSize;

        return await repository.GetUsersPaged(skip, take);
    }
}

public record GetUsersQuery(int Page, int PageSize) : IRequest<List<User>>;