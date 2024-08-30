namespace Shipping.Api.Models.Orders;

public class InsertRepresentativeInOrderVm
{
    public string OrderNo { get; set; }
    public Guid RepresentativeId { get; set; }
}