using FluentResults;
using MediatR;

namespace Shipping.Application.Features.UserManagement.Users.Queries.GetAdmins;

public class GetEmployeesRequest : IRequest<Result<List<GetUsersResponse>>>
{
    public string UserId { get; set; }

}