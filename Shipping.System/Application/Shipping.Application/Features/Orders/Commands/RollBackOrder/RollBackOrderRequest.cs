using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Orders.Commands.RollBackOrder;

public class RollBackOrderRequest : IRequest<Result<string>>
{
    public string OrderNo { get; set; }
    public string UserId { get; set; }
}