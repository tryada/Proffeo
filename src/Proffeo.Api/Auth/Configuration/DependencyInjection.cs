using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Proffeo.Api.Auth.Configuration;

public static class DependencyInjection
{
    public static void AddJwt(this IServiceCollection services)
    {
        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();
    }
}