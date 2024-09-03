using FluentResults;
using MediatR;

namespace Shipping.Application.Features.UserManagement.Permissions.Queries.GetPermissionsByRoleId;

public class GetAllPermissionsByUserIdRequest : IRequest<Result<List<GetPermissionsResponse>>>
{
    public Guid UserId { get; set; }
}