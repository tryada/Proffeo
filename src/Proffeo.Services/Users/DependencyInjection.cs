using Microsoft.Extensions.DependencyInjection;
using Proffeo.Services.Users.Utils;

namespace Proffeo.Services.Users;

internal static class DependencyInjection
{
    internal static IServiceCollection AddUsersServices(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<IEmailValidator, EmailValidator>();
    }
}