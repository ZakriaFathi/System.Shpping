using FluentResults;
using Shipping.Application.Features.Orders.Commands.AcceptanceOrders;
using Shipping.Application.Features.Orders.Commands.ChangeOrderStateByRepresentative;
using Shipping.Application.Features.Orders.Commands.CreateOrder;
using Shipping.Application.Features.Orders.Commands.DeleteOrder;
using Shipping.Application.Features.Orders.Commands.InsertRepresentativeInOrder;
using Shipping.Application.Features.Orders.Commands.RollBackOrder;
using Shipping.Application.Features.Orders.Queries.GetOrderByBranchId;
using Shipping.Application.Features.Orders.Queries.GetOrderByCustomerId;
using Shipping.Application.Features.Orders.Queries.GetOrderByRepresentativeId;
using Shipping.Application.Features.Orders.Queries;
using Shipping.Application.Features.Orders.Queries.ShearchOrder;

namespace Shipping.Application.Abstracts;

public interface IOrderRepository
{
    Task<Result<string>> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken);
    Task<Result<List<GetCustomerOrderResponse>>> GetOrderByCustomerAsync(GetOrderByCustomerRequest request, CancellationToken cancellationToken);
    Task<Result<List<GetRepresentativeOrderResponse>>> GetOrderByRepresentativeAsync(GetOrderByRepresentativeRequest request, CancellationToken cancellationToken);
    Task<Result<string>> AcceptanceOrderAsync(AcceptanceOrdersRequest request, CancellationToken cancellationToken);
    Task<Result<string>> RollBackOrderAsync(RollBackOrderRequest request, CancellationToken cancellationToken);
    Task<Result<string>> ChangeOrderStateByRepresentativeAsync(ChangeOrderStateByRepresentativeRequest request, CancellationToken cancellationToken);
    Task<Result<string>> InsertRepresentativeInOrderAsync(InsertRepresentativeInOrderRequest request, CancellationToken cancellationToken);
    Task<Result<List<GetOrderResponse>>> GetOrderByBranchIdAsync(GetOrderByBranchIdRequest request, CancellationToken cancellationToken);
    Task<Result<List<GetOrderResponse>>> ShearchOrderAsync(ShearchOrderRequest request, CancellationToken cancellationToken);
    Task<Result<string>> DeleteOrder(DeleteOrderRequest request, CancellationToken cancellationToken);

}