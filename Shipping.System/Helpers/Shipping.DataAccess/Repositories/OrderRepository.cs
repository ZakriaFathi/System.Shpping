using FluentResults;
using Microsoft.EntityFrameworkCore;
using Shipping.Application.Abstracts;
using Shipping.Application.Features.Branchs.Queries.GetBranchById;
using Shipping.Application.Features.Cities.Queries.GetCities;
using Shipping.Application.Features.Cities.Queries.GetCitiesByBranchId;
using Shipping.Application.Features.Orders.Commands.CreateOrder;
using Shipping.Application.Features.Orders.Queries.GetOrderByCustomerId;
using Shipping.DataAccess.Persistence.DataBase;
using Shipping.Domain.Entities;
using Shipping.Utils.Enums;

namespace Shipping.DataAccess.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ShippingDbContext _shippingDb;
    private readonly ICityRepository _cityRepository;
    private readonly IBranchRepository _branchRepository;

    public OrderRepository(ShippingDbContext shippingDb, ICityRepository cityRepository, IBranchRepository branchRepository)
    {
        _shippingDb = shippingDb;
        _cityRepository = cityRepository;
        _branchRepository = branchRepository;
    }
    public async Task<Result<string>> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetBranchByIdAsync(new GetBranchByIdRequest()
        {
            BranchId = request.BranchId
        }, cancellationToken);
        if (!branch.IsSuccess)
            return Result.Fail(branch.Errors.ToList());
        
        var allCities = await _cityRepository.GetCitiesByBranchIdAsync(new GetCitiesByBranchIdRequest()
        {
            BranchId = branch.Value.BranchId
        }, cancellationToken);
        if (!allCities.IsSuccess)
            return Result.Fail(allCities.Errors.ToList());

        var city = allCities.Value.Select(x => x.Cities.FirstOrDefault(y => y.CityId == request.CityId)).ToList();
        if (city.Count <= 0)
            return Result.Fail("المدينة غير موجودة");
        
        var sequenceNum =  await _shippingDb.GenerateSequance();
        if (!sequenceNum.IsSuccess) 
            return Result.Fail("يبدو أن خادم متوقف يرجى إعادة المحاولة في وقت لاحق");
        
        var order = new Order
        {
            OrderNo =  sequenceNum.Value,
            OrderState = OrderState.New,
            Dscription = request.Dscription,
            RecipientAddress = city[0]!.Name,
            CountOfItems = request.CountOfItems,
            SenderPhoneNo = request.SenderPhoneNo,
            RecipientPhoneNo = request.RecipientPhoneNo,
            CreatedAt = DateTime.Now,
            CustomerId = request.CustomerId,
            BranchId = request.BranchId,
            Price = city[0]!.Price
        };
        _shippingDb.Orders.Add(order);
        await _shippingDb.SaveChangesAsync(cancellationToken);
        return "تم انشاء الطلب بنجاح";
        
    }

    public async Task<Result<List<GetCustomerOrderResponse>>> GetOrderByCustomerIdAsync(GetOrderByCustomerIdRequest request, CancellationToken cancellationToken)
    {
        
        var orders = await _shippingDb.Orders
            .Where(x => x.CustomerId == request.CustomerId)
            .Include(o=> o.Branchs)
            .ThenInclude(u => u.Customers)
            .Select(x => new GetCustomerOrderResponse
            {
                OrderNo = x.OrderNo,
                OrderState = x.OrderState,
                Dscription = x.Dscription,
                RecipientAddress = x.RecipientAddress,
                CountOfItems = x.CountOfItems,
                SenderPhoneNo = x.SenderPhoneNo,
                RecipientPhoneNo = x.RecipientPhoneNo,
                Price = x.Price,
            }).ToListAsync(cancellationToken);
        return orders;
        
    }
}