using FluentResults;
using Shipping.Application.Features.Orders.Queries;
using Shipping.Application.Models;
using Shipping.Utils.Enums;

namespace Shipping.Application.Abstracts;

public interface ISherdOrderRepository
{
    Task<Result<List<GetOrderResponse>>> GetOrders(CancellationToken cancellationToken);
    Task<Result<CityVm>> GetCityId(Guid branchId, Guid cityId, CancellationToken cancellationToken);

    Task<Result<List<GetOrderResponse>>> GetOrderByState(OrderState state, Guid branchId,
        CancellationToken cancellationToken);
    
    Task<Result<List<GetOrderResponse>>> GetOrderByStateAndRepresentativeId(OrderState state,Guid representativeId, Guid branchId,
        CancellationToken cancellationToken);

    Task<Result<List<GetOrderResponse>>> GetOrderByRepresentativeId(Guid representativeId, Guid branchId,
        CancellationToken cancellationToken);
    
    Task<Result<List<GetOrderResponse>>> GetOrderByCityIdAndState(OrderState state, string cityName, Guid branchId,
        CancellationToken cancellationToken);

    Task<Result<List<GetOrderResponse>>> GetOrderByCityId(string cityName, Guid branchId,
        CancellationToken cancellationToken);
    
    Task<Result<List<GetOrderResponse>>> GetOrderByCityIdAndRepresentativeId(Guid representativeId, string cityName, Guid branchId,
        CancellationToken cancellationToken);
    Task<Result<List<GetOrderResponse>>> GetOrderBySenderPhoneNo(string senderPhoneNo, Guid branchId,
        CancellationToken cancellationToken);
    Task<Result<List<GetOrderResponse>>> GetOrderBySenderPhoneNoAndOrderNo(string senderPhoneNo, string orderNo, Guid branchId,
        CancellationToken cancellationToken);  
    Task<Result<List<GetOrderResponse>>> GetOrderByOrderNo(string orderNo, Guid branchId,
        CancellationToken cancellationToken); 

    
    Task<Result<List<GetOrderResponse>>> GetOrderByAll(OrderState state ,Guid representativeId , string cityName, Guid branchId, CancellationToken cancellationToken);
    

}