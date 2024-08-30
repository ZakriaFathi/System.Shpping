using FluentResults;
using MediatR;
using Shipping.Utils.Enums;

namespace Shipping.Application.Features.Orders.Commands.ChangeOrderStateByRepresentative;

public class ChangeOrderStateByRepresentativeRequest : IRequest<Result<string>>
{
    public string UserId { get; set; }
    public string OrderNo { get; set; }
    public OrderStateRepresentative OrderState { get; set; }
}