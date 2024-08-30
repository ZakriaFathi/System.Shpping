using System.Reflection;
using Microsoft.OpenApi.Extensions;
using Shipping.Utils.Enums;

namespace Shipping.Utils.Helper;

public static class RolePermissionHelper
{
    public static List<RolesAndPermissions> GetRolePermissions()
    {
        var rolePermissions = new List<RolesAndPermissions>();

        var roleType = typeof(RoleName);
        var roleFields = roleType.GetFields(BindingFlags.Public | BindingFlags.Static);

        foreach (var roleField in roleFields)
        {
            if (roleField.GetCustomAttribute(typeof(PermissionAttribute)) is not PermissionAttribute
                permissionAttribute) continue;
            var role = (RoleName)roleField.GetValue(null)!;
            var permissions = permissionAttribute.Permissions.ToList();
            
            List<Permissions> permissionsList = permissions.Select(x => new Permissions
            {
                PermissionId = Guid.NewGuid(),
                PermissionName = x.GetDisplayName()
            }).ToList();

            rolePermissions.Add(new RolesAndPermissions 
            {
                RoleId = Guid.NewGuid(),
                RoleName = role.ToString(),
                Permissions = permissionsList 
            });
        }
        return rolePermissions;
    }
}

public class RolesAndPermissions
{
    public Guid RoleId { get; set; }
    public string RoleName { get; set; }
    public List<Permissions> Permissions { get; set; }
}

public class Permissions
{
    public Guid PermissionId { get; set; }
    public string PermissionName { get; set; }
}
