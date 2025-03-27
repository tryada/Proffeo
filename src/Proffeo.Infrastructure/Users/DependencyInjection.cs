using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Proffeo.Services.Users.Interfaces;

namespace Proffeo.Infrastructure.Users;

internal static class DependencyInjection
{
    internal static IServiceCollection AddUsersDependencies(
        this IServiceCollection serviceCollection,
        ConfigurationManager configuration)
    {
        return serviceCollection
            .AddDbContext<UserDbContext>(options => 
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")))
            .AddTransient<IUserRepository, UserRepository>();
    }
}