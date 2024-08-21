using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderRequestHandler : IRequestHandler<CreateOrderRequest, Result<string>>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderRequestHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<string>> Handle(CreateOrderRequest request, CancellationToken cancellationToken)
        => await _orderRepository.CreateOrderAsync(request, cancellationToken);
}