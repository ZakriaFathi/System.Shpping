using FluentResults;
using MediatR;

namespace Shipping.Application.Features.UserManagement.Users.Commands.UpdateUserPermissions;

public class UpdateUserPermissionsRequest : IRequest<Result<string>>
{
    public Guid UserId { get; set; }
    public List<Guid> Permissions { get; set; } = new();
}