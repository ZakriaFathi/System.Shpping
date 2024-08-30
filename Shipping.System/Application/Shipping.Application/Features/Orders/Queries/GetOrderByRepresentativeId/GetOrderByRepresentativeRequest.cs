using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Orders.Queries.GetOrderByRepresentativeId;

public class GetOrderByRepresentativeRequest : IRequest<Result<List<GetRepresentativeOrderResponse>>>
{
    public string UserId { get; set; }
}