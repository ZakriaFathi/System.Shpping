using Shipping.Domain.Entities;

namespace Shipping.Domain;

public class UserPermission
{
    public Guid PermissionId { get; set; }
    public Permission Permission { get; set; }

    public Guid CustomerId { get; set; }
    public Customer Customer { get; set; }
}