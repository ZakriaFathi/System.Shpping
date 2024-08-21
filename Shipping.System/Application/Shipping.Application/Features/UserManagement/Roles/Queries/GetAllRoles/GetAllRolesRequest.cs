using FluentResults;
using MediatR;

namespace Shipping.Application.Features.UserManagement.Roles.Queries.GetAllRoles;

public class GetAllRolesRequest : IRequest<Result<List<GetRolesResponse>>>
{
    
}