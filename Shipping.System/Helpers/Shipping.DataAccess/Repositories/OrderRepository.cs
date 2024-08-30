using FluentResults;
using Microsoft.EntityFrameworkCore;
using Shipping.Application.Abstracts;
using Shipping.Application.Features.Orders.Commands.AcceptanceOrders;
using Shipping.Application.Features.Orders.Commands.ChangeOrderStateByEmployee;
using Shipping.Application.Features.Orders.Commands.ChangeOrderStateByRepresentative;
using Shipping.Application.Features.Orders.Commands.CreateOrder;
using Shipping.Application.Features.Orders.Commands.DeleteOrder;
using Shipping.Application.Features.Orders.Commands.InsertRepresentativeInOrder;
using Shipping.Application.Features.Orders.Queries.GetOrderByBranchId;
using Shipping.Application.Features.Orders.Queries.GetOrderByCustomerId;
using Shipping.Application.Features.Orders.Queries.GetOrderByRepresentativeId;
using Shipping.Application.Features.Orders.Queries;
using Shipping.Application.Features.Orders.Queries.ShearchOrder;
using Shipping.DataAccess.Persistence.DataBase;
using Shipping.Domain.Entities;
using Shipping.Utils.Enums;

namespace Shipping.DataAccess.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ShippingDbContext _shippingDb;
    private readonly ISherdOrderRepository _sherdOrder;

    public OrderRepository(ShippingDbContext shippingDb, ISherdOrderRepository sherdOrder)
    {
        _shippingDb = shippingDb;
        _sherdOrder = sherdOrder;
    }
    public async Task<Result<string>> CreateOrderAsync(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var customer = await _shippingDb.Customers.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId), cancellationToken);

        var city = await _sherdOrder.GetCityName(request.BranchId, request.CityId, cancellationToken);
        
        var sequenceNum =  await _shippingDb.GenerateSequance();
        if (!sequenceNum.IsSuccess) 
            return Result.Fail("يبدو أن خادم متوقف يرجى إعادة المحاولة في وقت لاحق");

        if (customer != null)
        {
            var order = new Order
            {
                OrderNo =  sequenceNum.Value,
                OrderState = OrderState.Pending,
                Dscription = request.Dscription,
                RecipientAddress = city.Value.Name,
                CountOfItems = request.CountOfItems,
                SenderPhoneNo = request.SenderPhoneNo,
                RecipientPhoneNo = request.RecipientPhoneNo,
                CreatedAt = DateTime.Now,
                CustomerId = customer.Id,
                BranchId = request.BranchId,
                Price = city.Value.Price
            };
            _shippingDb.Orders.Add(order);
        }

        await _shippingDb.SaveChangesAsync(cancellationToken);
        return "تم انشاء الطلب بنجاح";
        
    }

    public async Task<Result<List<GetCustomerOrderResponse>>> GetOrderByCustomerAsync(GetOrderByCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await _shippingDb.Customers.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId), cancellationToken);
        
        var orders = await _shippingDb.Orders
            .Where(x => customer != null && x.CustomerId == customer.Id)
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

        if (orders.Count <= 0)
            return Result.Fail("لايوجد طلبات");
        return orders;
        
    }

    public async Task<Result<List<GetRepresentativeOrderResponse>>> GetOrderByRepresentativeAsync(GetOrderByRepresentativeRequest request, CancellationToken cancellationToken)
    {
        var representatives = await _shippingDb.Representatives.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId), cancellationToken);

        var orders = await _shippingDb.Orders
            .Where(x => representatives != null && x.RepresentativesId == representatives.Id)
            .Select(x => new GetRepresentativeOrderResponse
            {
                OrderId = x.Id,
                OrderNo = x.OrderNo,
                RecipientAddress = x.RecipientAddress,
                Price = x.Price,
                RecipientPhoneNo = x.RecipientPhoneNo,
                SenderPhoneNo = x.SenderPhoneNo
            }).ToListAsync(cancellationToken);
        
        if (orders.Count <= 0)
            return Result.Fail("لايوجد طلبات");
        return orders;
    }

    public async Task<Result<string>> AcceptanceOrderAsync(AcceptanceOrdersRequest request, CancellationToken cancellationToken)
    {
        var employee = await _shippingDb.Employees.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId), cancellationToken);

        var order = await _shippingDb.Orders
            .FirstOrDefaultAsync(x => employee != null 
                                      && x.OrderNo == request.OrderNo 
                                      && x.BranchId == employee.BranchId, cancellationToken);
        if (order == null)
            return Result.Fail("الطلب غير موجود");
        
        order.OrderState = OrderState.InTheWarehouse;
        order.UpdatedAt = DateTime.Now;
       
        await _shippingDb.SaveChangesAsync(cancellationToken);
        return "تم قبول الطلب بنجاح";    }

    public async Task<Result<string>> ChangeOrderStateByEmployeeAsync(ChangeOrderStateByEmployeeRequest request, CancellationToken cancellationToken)
    {
        var employee = await _shippingDb.Employees.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId), cancellationToken);
        
        var order = await _shippingDb.Orders
            .FirstOrDefaultAsync(x => employee != null 
                                      && x.OrderNo == request.OrderNo 
                                      && x.BranchId == employee.BranchId, cancellationToken);
        if (order == null)
            return Result.Fail("الطلب غير موجود");

        OrderState? types = request.OrderState switch
        {
            OrderStateEmployee.DeliveredToTheRepresentative => OrderState.DeliveredToTheRepresentative,
            OrderStateEmployee.ReturnInTheWarehouse => OrderState.ReturnInTheWarehouse,
            OrderStateEmployee.ReturnInCustomer => OrderState.ReturnInCustomer,
            _ =>  null
        };

        if (types == null)
            return Result.Fail("الرجاء تحديد حالة الطلب");
        
        order.OrderState = types.Value;
        order.UpdatedAt = DateTime.Now;
        _shippingDb.Orders.Update(order);
        await _shippingDb.SaveChangesAsync(cancellationToken);
        return "تم تغيير حالة الطلب بنجاح";
    }

    public async Task<Result<string>> ChangeOrderStateByRepresentativeAsync(ChangeOrderStateByRepresentativeRequest request,
        CancellationToken cancellationToken)
    {
        var representatives = await _shippingDb.Representatives.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId), cancellationToken);
        
        var order = await _shippingDb.Orders
            .FirstOrDefaultAsync(x => representatives != null 
                                      && x.OrderNo == request.OrderNo 
                                      && x.BranchId == representatives.BranchId, cancellationToken);
        if (order == null)
            return Result.Fail("الطلب غير موجود");
        
        OrderState? types = request.OrderState switch
        {
            OrderStateRepresentative.Delivered => OrderState.Delivered,
            OrderStateRepresentative.Returning => OrderState.Returning,
            _ =>  null
        };

        if (types == null)
            return Result.Fail("الرجاء تحديد حالة الطلب");
        
        order.OrderState = types.Value;
        order.UpdatedAt = DateTime.Now;
        
        await _shippingDb.SaveChangesAsync(cancellationToken);
        return "تم تغيير حالة الطلب بنجاح";
    }

    public async Task<Result<string>> InsertRepresentativeInOrderAsync(InsertRepresentativeInOrderRequest request, CancellationToken cancellationToken)
    {
        var employee = await _shippingDb.Employees.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId), cancellationToken);
        
        var order = await _shippingDb.Orders
            .FirstOrDefaultAsync(x => employee != null 
                                      && x.OrderNo == request.OrderNo 
                                      && x.BranchId == employee.BranchId, cancellationToken);
        if (order == null)
            return Result.Fail("الطلب غير موجود");
        
        order.RepresentativesId = request.RepresentativeId;
        order.UpdatedAt = DateTime.Now;
        
        await _shippingDb.SaveChangesAsync(cancellationToken);
        return "تم تعيين المندوب بنجاح";
    }
    public async Task<Result<List<GetOrderResponse>>> GetOrderByBranchIdAsync(GetOrderByBranchIdRequest request, CancellationToken cancellationToken)
    {
        if (request.BranchId == Guid.Empty)
            return await _sherdOrder.GetOrders(cancellationToken);
        
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
        if (orders.Count <= 0)
            return Result.Fail<List<GetOrderResponse>>("لا يوجد طلبات");
        return orders;
    }

    public async Task<Result<List<GetOrderResponse>>> ShearchOrderAsync(ShearchOrderRequest request, CancellationToken cancellationToken)
    {
        var employee = await _shippingDb.Employees.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId), cancellationToken);
        
        var city = await _sherdOrder.GetCityName(employee.BranchId.Value, request.CityId, cancellationToken);

        OrderState? types = request.OrderState switch
        {
            OrderStateVm.Pending => OrderState.Pending,
            OrderStateVm.InTheWarehouse => OrderState.InTheWarehouse,
            OrderStateVm.DeliveredToTheRepresentative => OrderState.DeliveredToTheRepresentative,
            OrderStateVm.Delivered => OrderState.Delivered,
            OrderStateVm.Returning => OrderState.Returning,
            OrderStateVm.ReturnInTheWarehouse => OrderState.ReturnInTheWarehouse,
            OrderStateVm.ReturnInCustomer => OrderState.ReturnInCustomer,
            _ => null
        };
        
        
        var orderCriteria = (types == null, request.CityId == Guid.Empty, request.RepresentativeId == Guid.Empty);

        return orderCriteria switch
        {
            (true, true, true) => await _sherdOrder.GetOrders(cancellationToken),
            (false, true, true) => await _sherdOrder.GetOrderByState(types.Value, employee.BranchId.Value,
                cancellationToken),
            (false, false, true) => await _sherdOrder.GetOrderByCityNameAndState(types.Value, city.Value.Name, employee.BranchId.Value,
                cancellationToken),
            (false, true, false) => await _sherdOrder.GetOrderByStateAndRepresentative(types.Value,
                request.RepresentativeId, employee.BranchId.Value, cancellationToken),
            (true, false, true) => await _sherdOrder.GetOrderByCityName(city.Value.Name, employee.BranchId.Value, cancellationToken),
            (true, false, false) => await _sherdOrder.GetOrderByCityNameAndRepresentative(request.RepresentativeId,
                city.Value.Name, employee.BranchId.Value, cancellationToken), 
            (true, true, false) => await _sherdOrder.GetOrderByRepresentativeId(request.RepresentativeId, 
                employee.BranchId.Value, cancellationToken),
            _ => await _sherdOrder.GetOrderByAll(types.Value, request.RepresentativeId, city.Value.Name,
                employee.BranchId.Value, cancellationToken)
        };
        
    }

    public async Task<Result<string>> DeleteOrder(DeleteOrderRequest request, CancellationToken cancellationToken)
    {
        var employee = await _shippingDb.Employees.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId), cancellationToken);
        
        var order = await _shippingDb.Orders
            .FirstOrDefaultAsync(x => employee != null 
                                      && x.Id == request.Id 
                                      && x.BranchId == employee.BranchId, cancellationToken);
        if (order == null)
            return Result.Fail("الطلب غير موجود");
        
        _shippingDb.Orders.Remove(order);
        await _shippingDb.SaveChangesAsync(cancellationToken);
        
        return "تم الحذف";
    }
    
    
}