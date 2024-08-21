using FluentResults;
using MediatR;

namespace Shipping.Application.Features.UserManagement.Users.Queries.GetAdminsByBranchId;

public class GetAdminsByBranchIdRequest : IRequest<Result<List<GetUsersResponse>>>
{
    public Guid BranchId { get; set; }

}