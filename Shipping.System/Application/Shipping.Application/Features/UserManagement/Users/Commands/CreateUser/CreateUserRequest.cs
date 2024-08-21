using FluentResults;
using MediatR;
using Shipping.Utils.Enums;

namespace Shipping.Application.Features.UserManagement.Users.Commands.CreateUser;

public class CreateUserRequest : IRequest<Result<string>>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string PhoneNumber { get; set; }
    public UserTypeVm UserType { get; set; }
    public Guid BranchId { get; set; }

}