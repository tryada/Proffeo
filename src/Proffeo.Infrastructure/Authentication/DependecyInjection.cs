using Microsoft.Extensions.DependencyInjection;
using Proffeo.Services.Auth.Interfaces;

namespace Proffeo.Infrastructure.Authentication;

public static class DependecyInjection
{
    public static IServiceCollection AddAuthenticationDependencies(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddSingleton<IJwtProvider, JwtProvider>();
    }
}