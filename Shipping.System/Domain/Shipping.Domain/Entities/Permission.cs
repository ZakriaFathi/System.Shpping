namespace Shipping.Domain.Entities;

public class Permission
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public Guid RoleId { get; set; }
    public Role Role { get; set; }
    
    public ICollection<UserPermission> UserPermissions { get; set; }
}