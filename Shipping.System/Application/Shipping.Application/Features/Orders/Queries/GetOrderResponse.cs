using Shipping.Utils.Enums;

namespace Shipping.Application.Features.Orders.Queries;

public class GetOrderResponse
{
    public Guid OrderId { get; set; }
    public string OrderNo { get; set; }
    public string OrderState { get; set; }
    public string Dscription { get; set; }
    public int? CountOfItems { get; set; } 
    public decimal? OrderPrice { get; set; }
    public decimal? Price { get; set; }
    public string SenderPhoneNo { get; set; }
    public string RecipientPhoneNo { get; set; }
    public string BranchName { get; set; }
    public string RecipientAddress { get; set; }
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
    public string? PhoneNumber { get; set; }
}