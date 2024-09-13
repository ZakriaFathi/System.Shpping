using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.Orders.Queries.GetOrderByOrderNo;

public class GetOrderRequestByOrderNoHandler : IRequestHandler<GetOrderByOrderNoRequest, Result<List<GetOrderResponse>>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderRequestByOrderNoHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<List<GetOrderResponse>>> Handle(GetOrderByOrderNoRequest byOrderNoRequest,
        CancellationToken cancellationToken)
        => await _orderRepository.GetOrderByOrderNoAsync(byOrderNoRequest, cancellationToken);
}