using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Orders.Queries.GetWallet;

public class GetWalletRequest : IRequest<Result<string>>
{
    public string UserId { get; set; }
}