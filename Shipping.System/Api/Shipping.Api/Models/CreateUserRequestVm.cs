using Shipping.Utils.Enums;

namespace Shipping.Api.Models;

public class CreateUserRequestVm
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public UserTypeVm UserType { get; set; }
}