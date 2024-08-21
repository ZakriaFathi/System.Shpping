using FluentResults;
using MediatR;

namespace Shipping.Application.Features.UserManagement.Permissions.Commands.CreatePermission;

public class CreatePermissionRequest : IRequest<Result<string>>
{
    public string PermissionName{ get; set; }
    public Guid RoleId { get; set; }
}