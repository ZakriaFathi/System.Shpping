using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.Orders.Commands.ChangeOrderStateByEmployee;

public class ChangeOrderStateByEmployeeRequestHandler: IRequestHandler<ChangeOrderStateByEmployeeRequest, Result<string>>
{
    private readonly IOrderRepository _orderRepository;
    public ChangeOrderStateByEmployeeRequestHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<string>> Handle(ChangeOrderStateByEmployeeRequest request,
        CancellationToken cancellationToken)
        => await _orderRepository.ChangeOrderStateByEmployeeAsync(request, cancellationToken);
}