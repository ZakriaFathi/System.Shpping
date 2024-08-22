using FluentResults;
using Shipping.Application.Features.Orders.Commands.AcceptanceOrders;
using Shipping.Application.Features.Orders.Commands.ChangeOrderStateByEmployee;
using Shipping.Application.Features.Orders.Commands.ChangeOrderStateByRepresentative;
using Shipping.Application.Features.Orders.Commands.CreateOrder;
using Shipping.Application.Features.Orders.Commands.InsertRepresentativeInOrder;
using Shipping.Application.Features.Orders.Commands.ToRejectOrder;
using Shipping.Application.Features.Orders.Queries.GetOrderByBranchId;
using Shipping.Application.Features.Orders.Queries.GetOrderByCustomerId;
using Shipping.Application.Features.Orders.Queries.GetOrderByRepresentativeId;
using Shipping.Application.Features.Orders.Queries.GetOrders;

namespace Shipping.Application.Abstracts;

public interface IOrderRepository
{
    Task<Result<string>> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken);
    Task<Result<List<GetCustomerOrderResponse>>> GetOrderByCustomerIdAsync(GetOrderByCustomerIdRequest request, CancellationToken cancellationToken);
    Task<Result<List<GetRepresentativeOrderResponse>>> GetOrderByRepresentativeIdAsync(GetOrderByRepresentativeIdRequest request, CancellationToken cancellationToken);
    Task<Result<string>> AcceptanceOrderAsync(AcceptanceOrdersRequest request, CancellationToken cancellationToken);
    Task<Result<string>> ToRejectOrderAsync(ToRejectOrderRequest request, CancellationToken cancellationToken);
    Task<Result<string>> ChangeOrderStateByEmployeeAsync(ChangeOrderStateByEmployeeRequest request, CancellationToken cancellationToken);
    Task<Result<string>> ChangeOrderStateByRepresentativeAsync(ChangeOrderStateByRepresentativeRequest request, CancellationToken cancellationToken);
    Task<Result<string>> InsertRepresentativeInOrderAsync(InsertRepresentativeInOrderRequest request, CancellationToken cancellationToken);
    
    Task<Result<List<GetOrderResponse>>> GetOrdersAsync(GetOrdersRequest request, CancellationToken cancellationToken);
    Task<Result<List<GetOrderResponse>>> GetOrderByBranchIdAsync(GetOrderByBranchIdRequest request, CancellationToken cancellationToken);

}