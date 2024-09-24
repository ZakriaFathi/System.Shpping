using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.Orders.Queries.GetWallet;

public class GetWalletRequestHandler : IRequestHandler<GetWalletRequest, Result<string>>
{
    private readonly IOrderRepository _orderRepository;

    public GetWalletRequestHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<string>> Handle(GetWalletRequest request, CancellationToken cancellationToken)
        => await _orderRepository.GetWallet(request, cancellationToken);
}