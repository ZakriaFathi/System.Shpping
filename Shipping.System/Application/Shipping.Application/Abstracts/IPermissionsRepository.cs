using FluentResults;
using Shipping.Application.Features.UserManagement.Permissions;
using Shipping.Application.Features.UserManagement.Permissions.Commands.CreatePermission;
using Shipping.Application.Features.UserManagement.Permissions.Queries.GetAllPermissions;
using Shipping.Application.Features.UserManagement.Permissions.Queries.GetPermissionsByRoleId;

namespace Shipping.Application.Abstracts;

public interface IPermissionsRepository
{
    Task<Result<List<GetPermissionsResponse>>> GetAllPermissions(GetAllPermissionsRequest request, CancellationToken cancellationToken);
    Task<Result<List<GetPermissionsResponse>>> GetAllPermissionsByRoleId(GetAllPermissionsByRoleIdRequest request, CancellationToken cancellationToken);
    Task<Result<string>> CreatePermission(CreatePermissionRequest request, CancellationToken cancellationToken);
    Task<Result> DeleteUserPermissions(Guid userId, CancellationToken cancellationToken);
}