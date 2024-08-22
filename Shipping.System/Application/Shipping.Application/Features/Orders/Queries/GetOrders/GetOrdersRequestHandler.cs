using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.Orders.Queries.GetOrders;

public class GetOrdersRequestHandler : IRequestHandler<GetOrdersRequest, Result<List<GetOrderResponse>>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersRequestHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<List<GetOrderResponse>>> Handle(GetOrdersRequest request, CancellationToken cancellationToken)
        => await _orderRepository.GetOrdersAsync(request, cancellationToken);
}