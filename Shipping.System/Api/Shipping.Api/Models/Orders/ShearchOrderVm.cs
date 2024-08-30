using Shipping.Application.Features.Orders.Queries.ShearchOrder;
using Shipping.Utils.Enums;

namespace Shipping.Api.Models.Orders;

public class ShearchOrderVm
{
    public OrderStateVm OrderState { get; set; }
    public Guid CityId { get; set; }
    public Guid RepresentativeId { get; set; }
}