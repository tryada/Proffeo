using Microsoft.EntityFrameworkCore;
using Proffeo.Models.Users;

namespace Proffeo.Infrastructure.Users;

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}