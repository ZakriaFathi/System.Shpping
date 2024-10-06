namespace Shipping.Application.Models.UserManagement;

public class ChangePasswordCommand
{
    public Guid UserId { get; set; }
    public string OldPassWord { get; set; }
    public string NewPassWord { get; set; }
    public string ConfirmNewPassWord { get; set; }
}