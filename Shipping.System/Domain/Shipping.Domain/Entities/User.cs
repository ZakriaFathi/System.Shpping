using Shipping.Utils.Enums;

namespace Shipping.Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Password { get; set; }
    public string? UserName { get; set; }
    public UserType UserType { get; set; }
    public ActivateState ActivateState { get; set; }

    public ICollection<UserPermission> UserPermissions { get; set; }
    public Customer Customer { get; set; }
    public Employee Employee { get; set; }
    public Representative Representative { get; set; }

}