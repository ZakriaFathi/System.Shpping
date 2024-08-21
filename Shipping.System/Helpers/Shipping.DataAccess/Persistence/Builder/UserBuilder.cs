using Microsoft.EntityFrameworkCore;
using Shipping.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.DataAccess.Persistence.Builder;

public static class UserBuilder
{
    public static void AddUserBuilder(this DbContext dbContext, ModelBuilder builder)
    {
        builder.Entity<User>(b =>
        {
            b.HasKey(u => u.Id);
            b.Property(u => u.UserName).IsRequired();
            b.Property(u => u.Password).IsRequired();
            b.Property(u => u.ActivateState).IsRequired();
            b.Property(u => u.UserType).IsRequired();


            b.HasMany(u => u.UserPermissions)
                .WithOne(up => up.User)
                .HasForeignKey(up => up.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
