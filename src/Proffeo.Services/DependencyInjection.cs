using Microsoft.Extensions.DependencyInjection;
using Proffeo.Services.Users;

namespace Proffeo.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServicesDependencies(this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddUsersServices()
            .AddMediatR(configuration => {
                configuration.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });
    }
}