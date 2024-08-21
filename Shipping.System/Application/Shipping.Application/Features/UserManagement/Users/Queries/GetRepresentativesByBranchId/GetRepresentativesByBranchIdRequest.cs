using FluentResults;
using MediatR;

namespace Shipping.Application.Features.UserManagement.Users.Queries.GetRepresentativesByBranchId;

public class GetRepresentativesByBranchIdRequest : IRequest<Result<List<GetUsersResponse>>>
{
    public Guid BranchId { get; set; }

}