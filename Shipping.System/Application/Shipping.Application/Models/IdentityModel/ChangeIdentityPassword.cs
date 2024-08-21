namespace Shipping.Application.Models.IdentityModel;

public class ChangeIdentityPassword
{
    public string UserId { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmNewPassWord { get; set; }
}