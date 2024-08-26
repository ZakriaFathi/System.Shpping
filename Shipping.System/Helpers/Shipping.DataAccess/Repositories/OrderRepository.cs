using FluentResults;
using Microsoft.EntityFrameworkCore;
using Shipping.Application.Abstracts;
using Shipping.Application.Features.Branchs.Queries.GetBranchById;
using Shipping.Application.Features.Cities.Queries.GetCitiesByBranchId;
using Shipping.Application.Features.Orders.Commands.AcceptanceOrders;
using Shipping.Application.Features.Orders.Commands.ChangeOrderStateByEmployee;
using Shipping.Application.Features.Orders.Commands.ChangeOrderStateByRepresentative;
using Shipping.Application.Features.Orders.Commands.CreateOrder;
using Shipping.Application.Features.Orders.Commands.DeleteOrder;
using Shipping.Application.Features.Orders.Commands.InsertRepresentativeInOrder;
using Shipping.Application.Features.Orders.Commands.ToRejectOrder;
using Shipping.Application.Features.Orders.Queries.GetOrderByBranchId;
using Shipping.Application.Features.Orders.Queries.GetOrderByCustomerId;
using Shipping.Application.Features.Orders.Queries.GetOrderByRepresentativeId;
using Shipping.Application.Features.Orders.Queries.GetOrders;
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
            .Select(x => new GetCustomerOrderResponse
            {
                OrderId = x.Id,
                OrderNo = x.OrderNo,
                OrderState = x.OrderState,
                Dscription = x.Dscription,
                RecipientAddress = x.RecipientAddress,
                CountOfItems = x.CountOfItems,
                SenderPhoneNo = x.SenderPhoneNo,
                RecipientPhoneNo = x.RecipientPhoneNo,
                Price = x.Price,
                Representative = new RepresentativeVm
                {
                    Name = x.Representatives.Name,
                    PhoneNumber = x.Representatives.PhoneNumber
                }
            }).ToListAsync(cancellationToken);
        return orders;
        
    }

    public async Task<Result<List<GetRepresentativeOrderResponse>>> GetOrderByRepresentativeIdAsync(GetOrderByRepresentativeIdRequest request, CancellationToken cancellationToken)
    {
        var orders = await _shippingDb.Orders
            .Where(x => x.RepresentativesId == request.RepresentativeId)
            .Select(x => new GetRepresentativeOrderResponse
            {
                OrderId = x.Id,
                OrderNo = x.OrderNo,
                RecipientAddress = x.RecipientAddress,
                Price = x.Price,
                RecipientPhoneNo = x.RecipientPhoneNo,
                SenderPhoneNo = x.SenderPhoneNo
            }).ToListAsync(cancellationToken);
        return orders;
    }

    public async Task<Result<string>> AcceptanceOrderAsync(AcceptanceOrdersRequest request, CancellationToken cancellationToken)
    {
        var order = await _shippingDb.Orders
            .FirstOrDefaultAsync(x => x.OrderNo == request.OrderNo, cancellationToken);
        if (order == null)
            return Result.Fail("الطلب غير موجود");
        
        order.OrderState = OrderState.Pending;
        order.UpdatedAt = DateTime.Now;
        _shippingDb.Orders.Update(order);
        await _shippingDb.SaveChangesAsync(cancellationToken);
        return "تم قبول الطلب بنجاح";    }

    public async Task<Result<string>> ToRejectOrderAsync(ToRejectOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await _shippingDb.Orders
            .FirstOrDefaultAsync(x => x.OrderNo == request.OrderNo, cancellationToken);
        if (order == null)
            return Result.Fail("الطلب غير موجود");
        
        order.OrderState = OrderState.Canceled;
        order.UpdatedAt = DateTime.Now;
        _shippingDb.Orders.Update(order);
        await _shippingDb.SaveChangesAsync(cancellationToken);
        return "تم رفض الطلب";  
    }

    public async Task<Result<string>> ChangeOrderStateByEmployeeAsync(ChangeOrderStateByEmployeeRequest request, CancellationToken cancellationToken)
    {
        var order = await _shippingDb.Orders
            .FirstOrDefaultAsync(x => x.OrderNo == request.OrderNo, cancellationToken);
        if (order == null)
            return Result.Fail("الطلب غير موجود");

        var types = request.OrderState switch
        {
            OrderStateEmployee.InTheWarehouse => OrderState.InTheWarehouse,
            OrderStateEmployee.DeliveredToTheRepresentative => OrderState.DeliveredToTheRepresentative,
            _ => OrderState.Pending
        };
        
        order.OrderState = types;
        order.UpdatedAt = DateTime.Now;
        _shippingDb.Orders.Update(order);
        await _shippingDb.SaveChangesAsync(cancellationToken);
        return "تم تغيير حالة الطلب بنجاح";
    }

    public async Task<Result<string>> ChangeOrderStateByRepresentativeAsync(ChangeOrderStateByRepresentativeRequest request,
        CancellationToken cancellationToken)
    {
        var order = await _shippingDb.Orders
            .FirstOrDefaultAsync(x => x.OrderNo == request.OrderNo, cancellationToken);
        if (order == null)
            return Result.Fail("الطلب غير موجود");
        
        order.OrderState = OrderState.Delivered;
        order.UpdatedAt = DateTime.Now;
        _shippingDb.Orders.Update(order);
        await _shippingDb.SaveChangesAsync(cancellationToken);
        return "تم تغيير حالة الطلب بنجاح";
    }

    public async Task<Result<string>> InsertRepresentativeInOrderAsync(InsertRepresentativeInOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await _shippingDb.Orders
            .FirstOrDefaultAsync(x => x.OrderNo == request.OrderNo, cancellationToken);
        if (order == null)
            return Result.Fail("الطلب غير موجود");
        
        order.RepresentativesId = request.RepresentativeId;
        order.UpdatedAt = DateTime.Now;
        _shippingDb.Orders.Update(order);
        await _shippingDb.SaveChangesAsync(cancellationToken);
        return "تم تعيين المندوب بنجاح";
    }

    public async Task<Result<List<GetOrderResponse>>> GetOrdersAsync(GetOrdersRequest request, CancellationToken cancellationToken)
    {
        var orders = await _shippingDb.Orders
            .Select(x => new GetOrderResponse
            {
                OrderId = x.Id,
                OrderNo = x.OrderNo,
                OrderState = x.OrderState,
                RecipientAddress = x.RecipientAddress,
                Price = x.Price,
                RecipientPhoneNo = x.RecipientPhoneNo,
                SenderPhoneNo = x.SenderPhoneNo,
                BranchName = x.Branchs.Name,
                Dscription = x.Dscription,
                CountOfItems = x.CountOfItems,
                Representative = new RepresentativeVm
                {
                    Name = x.Representatives.Name,
                    PhoneNumber = x.Representatives.PhoneNumber
                },
                Customer = new CustomerVm
                {
                    Name = x.Customers.Name,
                    PhoneNumber = x.Customers.PhoneNumber
                }
            }).ToListAsync(cancellationToken);
        return orders;
    }

    public async Task<Result<List<GetOrderResponse>>> GetOrderByBranchIdAsync(GetOrderByBranchIdRequest request, CancellationToken cancellationToken)
    {
        var orders = await _shippingDb.Orders
            .Where(x => x.BranchId == request.BranchId)
            .Select(x => new GetOrderResponse
            {
                OrderId = x.Id,
                OrderNo = x.OrderNo,
                OrderState = x.OrderState,
                RecipientAddress = x.RecipientAddress,
                Price = x.Price,
                RecipientPhoneNo = x.RecipientPhoneNo,
                SenderPhoneNo = x.SenderPhoneNo,
                BranchName = x.Branchs.Name,
                Dscription = x.Dscription,
                CountOfItems = x.CountOfItems,
                Customer = new CustomerVm
                {
                    Name = x.Customers.Name,
                    PhoneNumber = x.Customers.PhoneNumber
                },
                Representative = new RepresentativeVm
                {
                    Name = x.Representatives.Name,
                    PhoneNumber = x.Representatives.PhoneNumber
                }
            }).ToListAsync(cancellationToken);
        return orders;
    }
    public async Task<Result<string>> DeleteOrder(DeleteOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await _shippingDb.Orders.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if(order == null)
            return Result.Fail("هذا الطلب غير موجود");
        
        _shippingDb.Orders.Remove(order);
        await _shippingDb.SaveChangesAsync();
        
        return "تم الحذف";
    }
}