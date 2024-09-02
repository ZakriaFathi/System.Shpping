using FluentResults;
using MediatR;

namespace Shipping.Application.Features.UserManagement.Users.Commands.CreateUserPermissions;

public class CreateUserPermissionsRequest : IRequest<Result<string>>
{
    public string UserId { get; set; }
    public List<Guid> Permissions { get; set; } = new();
}