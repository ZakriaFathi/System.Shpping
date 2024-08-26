using FluentResults;
using Shipping.Application.Features.UserManagement.Roles;
using Shipping.Application.Features.UserManagement.Roles.Commands.CreateRole;
using Shipping.Application.Features.UserManagement.Roles.Commands.DeleteRole;
using Shipping.Application.Features.UserManagement.Roles.Queries.GetAllRoles;
using Shipping.Application.Features.UserManagement.Roles.Queries.GetRolesByUserId;

namespace Shipping.Application.Abstracts;

public interface IRoleRepository
{
    Task<Result<List<GetRolesResponse>>> GetAllRoles(GetAllRolesRequest request, CancellationToken cancellationToken);
    Task<Result<List<GetRolesResponse>>> GetRolesByUserId(GetRolesByUserIdRequest request, CancellationToken cancellationToken);
    Task<Result<string>> CreateRole(CreateRoleRequest request, CancellationToken cancellationToken);
    Task<Result<string>> DeleteRole(DeleteRoleRequest request, CancellationToken cancellationToken);

}