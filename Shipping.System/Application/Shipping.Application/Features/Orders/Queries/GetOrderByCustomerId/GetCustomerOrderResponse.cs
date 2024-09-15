using Shipping.Application.Features.Orders.Queries;
using Shipping.Utils.Enums;

namespace Shipping.Application.Features.Orders.Queries.GetOrderByCustomerId;

public class GetCustomerOrderResponse
{
    public Guid OrderId { get; set; }
    public string OrderNo { get; set; }
    public OrderState OrderState { get; set; }
    public string Dscription { get; set; }
    public string RecipientAddress { get; set; }
    public int? CountOfItems { get; set; }
    public string SenderPhoneNo { get; set; }
    public string RecipientPhoneNo { get; set; }
    public decimal? Price { get; set; }
    public decimal? OrderPrice { get; set; }
    public RepresentativeVm Representative { get; set; }
}