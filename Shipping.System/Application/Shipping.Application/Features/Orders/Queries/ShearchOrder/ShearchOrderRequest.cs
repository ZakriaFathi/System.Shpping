using FluentResults;
using MediatR;
using Shipping.Application.Features.Orders.Queries;
using Shipping.Utils.Enums;

namespace Shipping.Application.Features.Orders.Queries.ShearchOrder;

public class ShearchOrderRequest : IRequest<Result<List<GetOrderResponse>>>
{
    public string UserId { get; set; }
    public OrderStateVm OrderState { get; set; }
    public Guid CityId { get; set; }
    public Guid RepresentativeId { get; set; }
}

public enum OrderStateVm
{
    Pending = 1,
    InTheWarehouse,
    DeliveredToTheRepresentative,
    Delivered,
    Returning,
    ReturnInTheWarehouse,
    ReturnInCustomer,
}