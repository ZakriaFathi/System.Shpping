using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;
using Shipping.Application.Features.Orders.Commands.AcceptanceOrders;

namespace Shipping.Application.Features.Orders.Commands.RollBackOrder;

public class RollBackOrderRequestHandler: IRequestHandler<RollBackOrderRequest, Result<string>>
{
    private readonly IOrderRepository _orderRepository;

    public RollBackOrderRequestHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<string>> Handle(RollBackOrderRequest request, CancellationToken cancellationToken)
        => await _orderRepository.RollBackOrderAsync(request, cancellationToken);
}