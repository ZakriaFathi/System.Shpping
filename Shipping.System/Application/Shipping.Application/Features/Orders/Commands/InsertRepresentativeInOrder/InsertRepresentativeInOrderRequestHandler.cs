using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.Orders.Commands.InsertRepresentativeInOrder;

public class InsertRepresentativeInOrderRequestHandler : IRequestHandler<InsertRepresentativeInOrderRequest, Result<string>>
{
    private readonly IOrderRepository _orderRepository;

    public InsertRepresentativeInOrderRequestHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<string>> Handle(InsertRepresentativeInOrderRequest request, CancellationToken cancellationToken)
        => await _orderRepository.InsertRepresentativeInOrderAsync(request, cancellationToken);
}