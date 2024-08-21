using Shipping.Utils.Enums;

namespace Shipping.Application.Models.IdentityModel;

public class InsertAndUpdateIdentityUser
{
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public UserType UserType { get; set; }
    public ActivateState ActivateState { get; set; }
}