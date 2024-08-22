using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.Orders.Commands.ChangeOrderStateByRepresentative;

public class ChangeOrderStateByRepresentativeRequestHandler : IRequestHandler<ChangeOrderStateByRepresentativeRequest, Result<string>>
{
    private readonly IOrderRepository _orderRepository;
    public ChangeOrderStateByRepresentativeRequestHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public async Task<Result<string>> Handle(ChangeOrderStateByRepresentativeRequest request, CancellationToken cancellationToken)
        => await _orderRepository.ChangeOrderStateByRepresentativeAsync(request, cancellationToken);
}