using Shipping.Utils.Enums;

namespace Shipping.Application.Models.UserManagement;

public class InsertAndUpdateUserCommnd
{
    public Guid UserId { get; set; }
    public string? UserName { get; set; }
    public UserType UserType { get; set; }
    public string Password { get; set; }
}