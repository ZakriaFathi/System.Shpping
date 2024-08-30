using FluentResults;
using MediatR;
using Shipping.Utils.Enums;

namespace Shipping.Application.Features.Orders.Commands.ChangeOrderStateByEmployee;

public class ChangeOrderStateByEmployeeRequest: IRequest<Result<string>>
{
    public string UserId { get; set; }
    public string OrderNo { get; set; }
    public OrderStateEmployee OrderState { get; set; }
}