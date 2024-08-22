using Shipping.Utils.Enums;

namespace Shipping.Application.Features.Orders.Queries.GetOrders;

public class GetOrderResponse
{
    public string OrderNo { get; set; }
    public OrderState OrderState { get; set; }
    public string Dscription { get; set; }
    public string RecipientAddress { get; set; }
    public int? CountOfItems { get; set; }
    public string SenderPhoneNo { get; set; }
    public string RecipientPhoneNo { get; set; }
    public decimal? Price { get; set; }
    public string BranchName { get; set; }
    public RepresentativeVm Representative { get; set; }
    public CustomerVm Customer { get; set; }
}

public class CustomerVm 
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
}
public class RepresentativeVm 
{
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
}