using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.Orders.Commands.ToRejectOrder;

public class ToRejectOrderRequestHandler : IRequestHandler<ToRejectOrderRequest, Result<string>>
{
    private readonly IOrderRepository _orderRepository;

    public ToRejectOrderRequestHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<string>> Handle(ToRejectOrderRequest request, CancellationToken cancellationToken)
        => await _orderRepository.ToRejectOrderAsync(request, cancellationToken);
}