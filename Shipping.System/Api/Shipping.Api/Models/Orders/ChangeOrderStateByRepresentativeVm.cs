using Shipping.Utils.Enums;

namespace Shipping.Api.Models.Orders;

public class ChangeOrderStateByRepresentativeVm
{
    public string OrderNo { get; set; }
    public OrderStateRepresentative OrderState { get; set; }
}