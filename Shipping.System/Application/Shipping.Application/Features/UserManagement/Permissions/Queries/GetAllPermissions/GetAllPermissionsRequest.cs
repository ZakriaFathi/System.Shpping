using FluentResults;
using MediatR;

namespace Shipping.Application.Features.UserManagement.Permissions.Queries.GetAllPermissions;

public class GetAllPermissionsRequest : IRequest<Result<List<GetPermissionsResponse>>>
{
    
}