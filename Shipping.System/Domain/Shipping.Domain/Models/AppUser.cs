using Microsoft.AspNetCore.Identity;
using Shipping.Utils.Enums;

namespace Shipping.Domain.Models;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public UserType UserType { get; set; }
    public ActivateState ActivateState { get; set; }
}