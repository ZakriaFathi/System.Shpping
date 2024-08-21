using Shipping.Domain.Entities;

namespace Shipping.Domain.Entities;

public class UserPermission
{
    public Guid PermissionId { get; set; }
    public Permission Permission { get; set; }

    public Guid CustomerId { get; set; }
    public User User { get; set; }
}