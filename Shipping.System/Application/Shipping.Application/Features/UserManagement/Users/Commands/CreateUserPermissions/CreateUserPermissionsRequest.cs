using FluentResults;
using MediatR;

namespace Shipping.Application.Features.UserManagement.Users.Commands.CreateUserPermissions;

public class CreateUserPermissionsRequest : IRequest<Result<string>>
{
    public string UserId { get; set; }
    public List<string> Permissions { get; set; } = new();
}