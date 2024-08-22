using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Orders.Queries.GetOrders;

public class GetOrdersRequest : IRequest<Result<List<GetOrderResponse>>>
{
    
}