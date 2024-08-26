namespace Shipping.Application.Models.UserManagement;

public class UpdatePasswordCommnd
{
    public string UserName { get; set; }

    public string NewPassword { get; set; }
}