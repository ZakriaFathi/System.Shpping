using FluentResults;
using MediatR;

namespace Shipping.Application.Features.UserManagement.Roles.Queries.GetRolesByUserId;

public class GetRolesByUserIdRequest : IRequest<Result<List<GetRolesResponse>>>
{
    public Guid UserId { get; set; }
}