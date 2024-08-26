using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderRequestHandler : IRequestHandler<DeleteOrderRequest, Result<string>>
{
    private readonly IOrderRepository _orderRepository;

    public DeleteOrderRequestHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<string>> Handle(DeleteOrderRequest request, CancellationToken cancellationToken)
        => await _orderRepository.DeleteOrder(request, cancellationToken);
}