using FluentResults;
using MediatR;
using Shipping.Application.Features.Orders.Queries.GetOrders;

namespace Shipping.Application.Features.Orders.Queries.GetOrderByBranchId;

public class GetOrderByBranchIdRequest : IRequest<Result<List<GetOrderResponse>>>
{
    public Guid BranchId { get; set; }
}