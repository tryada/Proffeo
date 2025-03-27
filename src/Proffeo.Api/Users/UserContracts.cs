using Proffeo.Models.Users;
using Proffeo.Services.Users.Commands;
using Proffeo.Services.Users.Queries;

namespace Proffeo.Api.Users;

public record UserResponse(Guid Id,  string Name, string Email);

public record CreateUserRequest(string Name, string Email);

public record UpdateUserRequest(string Name, string Email);

internal static class UserContractsExtensions
{
    public static CreateUserCommand ToCommand(this CreateUserRequest request) => new(request.Name, request.Email);
    public static UpdateUserCommand ToCommand(this UpdateUserRequest request, Guid id) => new(id, request.Name, request.Email);
    public static List<UserResponse> ToResponse(this List<User> users) => users.Select(user => new UserResponse(user.Id, user.Name, user.Email)).ToList();
    public static UserResponse ToResponse(this User user) => new(user.Id, user.Name, user.Email);
    
    public static GetUsersQuery GetUsersQuery(int page, int pageSize) => new(page, pageSize);
    public static GetUserQuery GetUserQuery(Guid id) => new(id);
    public static DeleteUserCommand DeleteUserCommand(Guid id) => new(id);
}