using FluentResults;
using Shipping.Application.Features.Orders.Commands.CreateOrder;
using Shipping.Application.Features.Orders.Queries.GetOrderByCustomerId;

namespace Shipping.Application.Abstracts;

public interface IOrderRepository
{
    Task<Result<string>> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken);
    Task<Result<List<GetCustomerOrderResponse>>> GetOrderByCustomerIdAsync(GetOrderByCustomerIdRequest request, CancellationToken cancellationToken);
}