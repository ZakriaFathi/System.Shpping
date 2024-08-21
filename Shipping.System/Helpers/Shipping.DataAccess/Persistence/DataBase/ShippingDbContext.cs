using FluentResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Shipping.DataAccess.Persistence.Builder;
using Shipping.Domain;
using Shipping.Domain.Entities;

namespace Shipping.DataAccess.Persistence.DataBase;

public class ShippingDbContext : DbContext
{
    public DbSet<Branch> Branchs { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Permission> Permissions { get; set; } 
    public DbSet<Role> Roles { get; set; } 
    public DbSet<UserPermission> UserPermissions { get; set; } 
    
    
    public ShippingDbContext(DbContextOptions<ShippingDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        this.AddUserPermissionsBuilder(modelBuilder);
        base.OnModelCreating(modelBuilder);
    }

    public async Task<Result<string>> GenerateSequance()
    {
        var result = new SqlParameter("@result", System.Data.SqlDbType.BigInt)
        {
            Direction = System.Data.ParameterDirection.Output
        };
        await Database.ExecuteSqlInterpolatedAsync($"SELECT {result} = (NEXT VALUE FOR dbo.[ContractNoSequence])");
        return MapSeqNumber($"{(long)result.Value}");
    }

    private static string MapSeqNumber(string seqNumber)
        => seqNumber.PadLeft(7, '1');
}