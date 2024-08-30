using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderRequest : IRequest<Result<string>>
{ 
    public string UserId { get; set; }
    public string Dscription { get; set; }
    public Guid CityId { get; set; }
    public int? CountOfItems { get; set; }
    public string SenderPhoneNo { get; set; }
    public string RecipientPhoneNo { get; set; }
    public Guid BranchId { get; set; }
}