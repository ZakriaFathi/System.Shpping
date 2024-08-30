using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.Orders.Queries.GetOrderByCustomerId;

public class GetOrderByCustomerIdRequestHandler : IRequestHandler<GetOrderByCustomerRequest , Result<List<GetCustomerOrderResponse>>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByCustomerIdRequestHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<List<GetCustomerOrderResponse>>> Handle(GetOrderByCustomerRequest request, CancellationToken cancellationToken)
        => await _orderRepository.GetOrderByCustomerAsync(request, cancellationToken);
}