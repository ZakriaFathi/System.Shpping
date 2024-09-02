using FluentResults;
using MediatR;
using Shipping.Utils.Enums;

namespace Shipping.Application.Features.UserManagement.Users.Commands.UpdateUser;

public class UpdateUserRequest : IRequest<Result<string>>
{
    public string UserId { get; set; }
    public string FristName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
}