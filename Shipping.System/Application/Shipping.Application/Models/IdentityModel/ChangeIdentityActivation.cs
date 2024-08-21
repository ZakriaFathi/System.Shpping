using Shipping.Utils.Enums;

namespace Shipping.Application.Models.IdentityModel;

public class ChangeIdentityActivation
{
    public string UserId { get; set; }
    public ActivateState State { get; set; }
}