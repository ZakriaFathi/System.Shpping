using FluentResults;
using MediatR;

namespace Shipping.Application.Features.UserManagement.Permissions.Queries.GetPermissionsByRoleId;

public class GetAllPermissionsByRoleIdRequest : IRequest<Result<List<GetPermissionsResponse>>>
{
    public Guid RoleId { get; set; }
}