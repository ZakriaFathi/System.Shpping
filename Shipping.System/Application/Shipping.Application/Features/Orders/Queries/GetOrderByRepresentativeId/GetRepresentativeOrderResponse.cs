using Shipping.Utils.Enums;

namespace Shipping.Application.Features.Orders.Queries.GetOrderByRepresentativeId;

public class GetRepresentativeOrderResponse
{
    public Guid OrderId { get; set; }
    public string OrderNo { get; set; }
    public decimal? OrderPrice { get; set; }
    public decimal? Price { get; set; }
    public string RecipientAddress { get; set; }
    public string SenderPhoneNo { get; set; }
    public string RecipientPhoneNo { get; set; }
    public OrderState OrderState { get; set; }
}