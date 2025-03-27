using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Proffeo.Infrastructure.Users;

namespace Proffeo.Infrastructure;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        if (dbContext.Database.GetPendingMigrations().Any())
            dbContext.Database.Migrate();
    }
}