using Microsoft.EntityFrameworkCore;
using Shipping.Domain.Entities;

namespace Shipping.DataAccess.Persistence.Builder;

public static class RepresentativeBuilder
{
    public static void AddRepresentativeBuilder(this DbContext dbContext, ModelBuilder builder)
    {
        builder.Entity<Representative>(b =>
        {
            b.HasKey(e => e.Id);
            b.Property(e => e.Name).IsRequired();
            b.Property(e => e.PhoneNumber).IsRequired();
            b.Property(e => e.Address).IsRequired();
            b.Property(e => e.UserId).IsRequired();
            b.HasOne<User>()
                .WithOne()
                .HasForeignKey<Representative>(e => e.UserId)
                .IsRequired();
        });
        
    }
}