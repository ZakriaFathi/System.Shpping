using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.Orders.Commands.AcceptanceOrders;

public class AcceptanceOrdersRequestHandler : IRequestHandler<AcceptanceOrdersRequest, Result<string>>
{
    private readonly IOrderRepository _orderRepository;

    public AcceptanceOrdersRequestHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<string>> Handle(AcceptanceOrdersRequest request, CancellationToken cancellationToken)
        => await _orderRepository.AcceptanceOrderAsync(request, cancellationToken);
}