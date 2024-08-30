namespace Shipping.Api.Models.Orders;

public class CreateOrderVm
{
    public string Dscription { get; set; }
    public Guid CityId { get; set; }
    public int? CountOfItems { get; set; }
    public string SenderPhoneNo { get; set; }
    public string RecipientPhoneNo { get; set; }
    public Guid BranchId { get; set; }
}