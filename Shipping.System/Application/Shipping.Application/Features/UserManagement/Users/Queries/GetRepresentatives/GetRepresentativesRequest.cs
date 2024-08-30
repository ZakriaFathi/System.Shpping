using FluentResults;
using MediatR;

namespace Shipping.Application.Features.UserManagement.Users.Queries.GetRepresentatives;

public class GetRepresentativesRequest : IRequest<Result<List<GetUsersResponse>>>
{
    public string UserId { get; set; }
}