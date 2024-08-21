using Microsoft.EntityFrameworkCore;
using Shipping.Domain.Entities;

namespace Shipping.DataAccess.Persistence.Builder;

public static class EmployeeBuilder
{
    public static void AddEmployeeBuilder(this DbContext dbContext, ModelBuilder builder)
    {
        builder.Entity<Employee>(b =>
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Name).IsRequired();
            b.Property(e => e.PhoneNumber).IsRequired();
            b.Property(e => e.Address).IsRequired();
            b.Property(e => e.UserId).IsRequired();
            b.HasOne<User>()
                .WithOne()
                .HasForeignKey<Employee>(e => e.UserId)
                .IsRequired();
        });
        
    }
}