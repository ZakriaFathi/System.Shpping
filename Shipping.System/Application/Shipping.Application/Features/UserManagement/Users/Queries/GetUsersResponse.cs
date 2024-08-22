using Shipping.Utils.Enums;

namespace Shipping.Application.Features.UserManagement.Users.Queries;

public class GetUsersResponse
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string? UserName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Password { get; set; }
    public UserType UserType { get; set; }
    public ActivateState ActivateState { get; set; }
    public string BranchName { get; set; }
}