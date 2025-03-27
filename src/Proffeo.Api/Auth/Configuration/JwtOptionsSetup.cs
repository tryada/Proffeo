using Microsoft.Extensions.Options;
using Proffeo.Infrastructure.Authentication;

namespace Proffeo.Api.Auth.Configuration;

public class JwtOptionsSetup(IConfiguration configuration) 
    : IConfigureOptions<JwtOptions>
{
    private const string SectionName = "Jwt";

    public void Configure(JwtOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}