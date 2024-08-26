namespace Shipping.Application.Models.IdentityModel;

public class ResetIdentityPassword
{
    public string UserName { get; set; }

    public string NewPassword { get; set; }
    public string ConfiramNewPassword { get; set; }
}