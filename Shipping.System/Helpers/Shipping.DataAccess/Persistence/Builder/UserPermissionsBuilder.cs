using Microsoft.EntityFrameworkCore;
using Shipping.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.DataAccess.Persistence.Builder
{
    public static class UserPermissionsBuilder
    {
        public static void AddUserPermissionsBuilder(this DbContext dbContext, ModelBuilder builder)
        {
            builder.Entity<UserPermission>(b =>
            {
                b.HasKey(up => new { up.CustomerId, up.PermissionId });
                b.HasOne(up => up.User)
                    .WithMany(u => u.UserPermissions)
                    .HasForeignKey(up => up.CustomerId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(up => up.Permission)
                    .WithMany(p => p.UserPermissions)
                    .HasForeignKey(rp => rp.PermissionId)
                    .OnDelete(DeleteBehavior.Restrict);

            });
        }
    }
}
