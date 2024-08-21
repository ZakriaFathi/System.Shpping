using Shipping.Utils.Enums;

namespace Shipping.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Password { get; set; }
    public string? Email { get; set; }
    public string UserName { get; set; }
    public UserType UserType { get; set; }
    public ActivateState ActivateState { get; set; }

    public ICollection<UserPermission> UserPermissions { get; set; }
    public ICollection<Order> Orders { get; set; }

    public Guid? BranchId { get; set; } 
    public Branch Branch { get; set; }
}