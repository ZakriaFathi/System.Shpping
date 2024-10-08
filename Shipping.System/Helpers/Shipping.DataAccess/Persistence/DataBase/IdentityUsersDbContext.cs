using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shipping.Domain.Models;

namespace Shipping.DataAccess.Persistence.DataBase;

public class IdentityUsersDbContext : IdentityDbContext<AppUser>
{
    public IdentityUsersDbContext(DbContextOptions<IdentityUsersDbContext> Options) : base(Options) { }
}