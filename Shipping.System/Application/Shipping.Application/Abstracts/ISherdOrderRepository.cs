using FluentResults;
using Shipping.Application.Features.Orders.Queries;
using Shipping.Application.Models;
using Shipping.Utils.Enums;

namespace Shipping.Application.Abstracts;

public interface ISherdOrderRepository
{
    Task<Result<List<GetOrderResponse>>> GetOrders(CancellationToken cancellationToken);
    Task<Result<CityVm>> GetCityName(Guid branchId, Guid cityId, CancellationToken cancellationToken);

    Task<Result<List<GetOrderResponse>>> GetOrderByState(OrderState state, Guid branchId,
        CancellationToken cancellationToken);
    
    Task<Result<List<GetOrderResponse>>> GetOrderByStateAndRepresentative(OrderState state,Guid representativeId, Guid branchId,
        CancellationToken cancellationToken);

    Task<Result<List<GetOrderResponse>>> GetOrderByRepresentativeId(Guid representativeId, Guid branchId,
        CancellationToken cancellationToken);
    
    Task<Result<List<GetOrderResponse>>> GetOrderByCityNameAndState(OrderState state, string cityName, Guid branchId,
        CancellationToken cancellationToken);

    Task<Result<List<GetOrderResponse>>> GetOrderByCityName(string cityName, Guid branchId,
        CancellationToken cancellationToken);
    
    Task<Result<List<GetOrderResponse>>> GetOrderByCityNameAndRepresentative(Guid representativeId, string cityName, Guid branchId,
        CancellationToken cancellationToken);
    
    Task<Result<List<GetOrderResponse>>> GetOrderByAll(OrderState state ,Guid representativeId , string cityName, Guid branchId, CancellationToken cancellationToken);
    

}