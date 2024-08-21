using FluentResults;
using MediatR;

namespace Shipping.Application.Features.UserManagement.Users.Queries.GetAdmins;

public class GetAdminsRequest : IRequest<Result<List<GetUsersResponse>>>
{
    
}