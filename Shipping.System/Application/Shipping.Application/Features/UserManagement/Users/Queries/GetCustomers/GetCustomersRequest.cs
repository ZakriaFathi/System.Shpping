using FluentResults;
using MediatR;

namespace Shipping.Application.Features.UserManagement.Users.Queries.GetCustomers;

public class GetCustomersRequest : IRequest<Result<List<GetCustomersResponse>>>
{
    
}