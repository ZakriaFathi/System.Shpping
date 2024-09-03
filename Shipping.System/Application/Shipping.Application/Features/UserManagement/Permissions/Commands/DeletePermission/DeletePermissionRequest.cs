using FluentResults;
using MediatR;

namespace Shipping.Application.Features.UserManagement.Permissions.Commands.DeletePermission;

public class DeletePermissionRequest : IRequest<Result<string>>
{
    public Guid UserId { get; set; }
}