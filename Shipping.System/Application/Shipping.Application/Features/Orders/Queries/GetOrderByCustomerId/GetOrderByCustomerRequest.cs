using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Orders.Queries.GetOrderByCustomerId;

public class GetOrderByCustomerRequest :IRequest<Result<List<GetCustomerOrderResponse>>>
{
    public string UserId { get; set; }
}