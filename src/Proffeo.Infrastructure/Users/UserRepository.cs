using Microsoft.EntityFrameworkCore;
using Proffeo.Models.Users;
using Proffeo.Services.Users.Interfaces;

namespace Proffeo.Infrastructure.Users;

internal class UserRepository(UserDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id) => await dbContext.Users.FindAsync(id);
    
    public async Task<List<User>> GetUsersPaged(int skip, int take)
    {
        return await dbContext.Users
            .OrderBy(u => u.Id)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await dbContext.Users.AddAsync(user, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(User user, CancellationToken cancellationToken = default)
    {
        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}