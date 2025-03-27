using Proffeo.Api.Auth.Configuration;

namespace Proffeo.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddJwt();
        serviceCollection.AddControllers();
        return serviceCollection;
    }
}