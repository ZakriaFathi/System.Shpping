using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Orders.Queries.GetOrderByCustomerId;

public class GetOrderByCustomerIdRequest :IRequest<Result<List<GetCustomerOrderResponse>>>
{
    public Guid CustomerId { get; set; }
}