using Shipping.Utils.Enums;

namespace Shipping.Application.Models.UserManagement;

public class ChangeUserActivationCommnd
{
    public Guid UserId { get; set; }
    public ActivateState State { get; set; }
}