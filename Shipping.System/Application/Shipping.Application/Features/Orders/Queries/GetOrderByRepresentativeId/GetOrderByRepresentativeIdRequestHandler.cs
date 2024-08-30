using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.Orders.Queries.GetOrderByRepresentativeId;

public class GetOrderByRepresentativeIdRequestHandler : IRequestHandler<GetOrderByRepresentativeRequest, Result<List<GetRepresentativeOrderResponse>>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByRepresentativeIdRequestHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<List<GetRepresentativeOrderResponse>>> Handle(GetOrderByRepresentativeRequest request, CancellationToken cancellationToken)
        => await _orderRepository.GetOrderByRepresentativeAsync(request, cancellationToken);
}