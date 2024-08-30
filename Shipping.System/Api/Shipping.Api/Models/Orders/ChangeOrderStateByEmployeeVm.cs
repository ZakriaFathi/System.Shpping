using Shipping.Utils.Enums;

namespace Shipping.Api.Models.Orders;

public class ChangeOrderStateByEmployeeVm
{
    public string OrderNo { get; set; }
    public OrderStateEmployee OrderState { get; set; }
}