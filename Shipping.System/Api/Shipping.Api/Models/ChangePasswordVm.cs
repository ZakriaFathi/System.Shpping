namespace Shipping.Api.Models;

public class ChangePasswordVm
{
    public string OldPassWord { get; set; }
    public string NewPassWord { get; set; }
    public string ConfirmNewPassWord { get; set; }
}
