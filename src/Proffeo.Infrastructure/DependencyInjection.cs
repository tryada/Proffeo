using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Proffeo.Infrastructure.Authentication;
using Proffeo.Infrastructure.Users;

namespace Proffeo.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection,
        ConfigurationManager configuration)
    {
        return serviceCollection
            .AddAuthenticationDependencies()
            .AddUsersDependencies(configuration);
    }
}