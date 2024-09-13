using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Orders.Queries.GetOrderByOrderNo;

public class GetOrderByOrderNoRequest : IRequest<Result<List<GetOrderResponse>>>
{
    public string UserId { get; set; }
    public string OrderNo { get; set; }
}