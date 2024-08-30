using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;
using Shipping.Application.Features.Orders.Queries;

namespace Shipping.Application.Features.Orders.Queries.ShearchOrder;

public class ShearchOrderRequestHandler : IRequestHandler<ShearchOrderRequest, Result<List<GetOrderResponse>>>
{
    private readonly IOrderRepository _orderRepository;

    public ShearchOrderRequestHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public async Task<Result<List<GetOrderResponse>>> Handle(ShearchOrderRequest request, CancellationToken cancellationToken)
    => await _orderRepository.ShearchOrderAsync(request, cancellationToken);
}