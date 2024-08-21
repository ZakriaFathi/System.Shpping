using FluentResults;
using MediatR;

namespace Shipping.Application.Features.UserManagement.Users.Queries.GetAdminsByBranchId;

public class GetEmployeesByBranchIdRequest : IRequest<Result<List<GetUsersResponse>>>
{
    public Guid BranchId { get; set; }

}