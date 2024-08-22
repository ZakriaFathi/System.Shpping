using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Orders.Commands.ToRejectOrder;

public class ToRejectOrderRequest : IRequest<Result<string>>
{
    public string OrderNo { get; set; }
}