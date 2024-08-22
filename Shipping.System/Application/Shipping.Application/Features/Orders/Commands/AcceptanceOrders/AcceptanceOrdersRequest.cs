using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Orders.Commands.AcceptanceOrders;

public class AcceptanceOrdersRequest : IRequest<Result<string>>
{
    public string OrderNo { get; set; }
}