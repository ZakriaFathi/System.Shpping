using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;
using Shipping.Application.Features.Orders.Queries;

namespace Shipping.Application.Features.Orders.Queries.GetOrderByBranchId;

public class GetOrderByBranchIdRequestHandler : IRequestHandler<GetOrderByBranchIdRequest, Result<List<GetOrderResponse>>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByBranchIdRequestHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<List<GetOrderResponse>>> Handle(GetOrderByBranchIdRequest request, CancellationToken cancellationToken)
        => await _orderRepository.GetOrderByBranchIdAsync(request, cancellationToken);
}