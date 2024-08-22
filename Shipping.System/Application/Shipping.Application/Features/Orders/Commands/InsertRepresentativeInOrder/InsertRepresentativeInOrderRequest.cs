using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Orders.Commands.InsertRepresentativeInOrder;

public class InsertRepresentativeInOrderRequest : IRequest<Result<string>>
{
    public string OrderNo { get; set; }
    public Guid RepresentativeId { get; set; }
}