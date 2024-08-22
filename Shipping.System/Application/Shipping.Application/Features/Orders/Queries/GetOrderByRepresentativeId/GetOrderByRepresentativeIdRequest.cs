using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Orders.Queries.GetOrderByRepresentativeId;

public class GetOrderByRepresentativeIdRequest : IRequest<Result<List<GetRepresentativeOrderResponse>>>
{
    public Guid RepresentativeId { get; set; }
}