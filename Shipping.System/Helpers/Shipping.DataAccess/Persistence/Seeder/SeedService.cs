using Microsoft.EntityFrameworkCore;
using Shipping.DataAccess.Persistence.DataBase;
using Shipping.Domain.Entities;
using Shipping.Utils.Helper;

namespace Shipping.DataAccess.Persistence.Seeder;

public class SeedService
{
    private readonly ShippingDbContext _dbContext;

    public SeedService(ShippingDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Seed()
    {
        await SeedRolesAndPermissions();
    }
    
    private async Task SeedRolesAndPermissions()
    {
        var rolesPermissions = RolePermissionHelper.GetRolePermissions();
        
        var dbPermissions = await _dbContext.Permissions.ToListAsync();
        var dbRoles = await _dbContext.Roles.ToListAsync();
        
        var roles = new List<Role>();
        var permissions = new List<Permission>();
        rolesPermissions.ForEach(x =>
        {
            roles.Add(Role.Create(x.RoleId, x.RoleName));
            x.Permissions.ForEach(p => permissions.Add(Permission.Create(p.PermissionId,p.PermissionName,x.RoleId)));
        });

        var rolesToAdd = roles.ExceptBy(dbRoles.Select(x => x.Id),x => x.Id);
        var permissionsToAdd = permissions.ExceptBy(dbPermissions.Select(x => x.Id),x => x.Id);
        
        await _dbContext.Roles.AddRangeAsync(rolesToAdd);
        await _dbContext.Permissions.AddRangeAsync(permissionsToAdd);
        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.InnerException?.Message);
            throw; // Rethrow or handle accordingly
        }    
    }
}