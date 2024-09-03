using FluentResults;
using Shipping.Application.Features.UserManagement.Permissions;
using Shipping.Application.Features.UserManagement.Permissions.Commands.DeletePermission;
using Shipping.Application.Features.UserManagement.Permissions.Queries.GetAllPermissions;
using Shipping.Application.Features.UserManagement.Permissions.Queries.GetPermissionsByRoleId;

namespace Shipping.Application.Abstracts;

public interface IPermissionsRepository
{
    Task<Result<List<GetPermissionsResponse>>> GetAllPermissions(GetAllPermissionsRequest request, CancellationToken cancellationToken);

    Task<Result<List<GetPermissionsResponse>>> GetPermissionsByUserId(GetAllPermissionsByUserIdRequest request, CancellationToken cancellationToken);
    Task<Result> DeleteUserPermissions(DeletePermissionRequest request, CancellationToken cancellationToken);
}