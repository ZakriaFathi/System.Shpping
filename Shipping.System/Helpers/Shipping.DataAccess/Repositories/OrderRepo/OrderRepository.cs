using FluentResults;
using Microsoft.EntityFrameworkCore;
using Shipping.Application.Abstracts;
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
using Shipping.Application.Features.Orders.Queries.GetOrderByOrderNo;
using Shipping.Application.Features.Orders.Queries.GetWallet;
using Shipping.Application.Features.Orders.Queries.ShearchOrder;
using Shipping.DataAccess.Persistence.DataBase;
using Shipping.Domain.Entities;
using Shipping.Utils.Enums;

namespace Shipping.DataAccess.Repositories.OrderRepo;

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
        var customer =
            await _shippingDb.Customers.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId),
                cancellationToken);

        var city = await _sherdOrder.GetCityId(request.BranchId, request.CityId, cancellationToken);

        // var sequenceNum = await _shippingDb.GenerateSequance();
        // if (!sequenceNum.IsSuccess)
        //     return Result.Fail("يبدو أن خادم متوقف يرجى إعادة المحاولة في وقت لاحق");.
        
        Random rnd = new Random();

        var sequenceNum = rnd.Next(00001, 99999); 

        if (customer != null)
        {
            var order = new Order
            {
                OrderNo = sequenceNum.ToString(),
                OrderState = OrderState.Pending,
                Dscription = request.Dscription,
                RecipientAddress = city.Value.Name,
                CountOfItems = request.CountOfItems,
                SenderPhoneNo = customer.PhoneNumber,
                RecipientPhoneNo = request.RecipientPhoneNo,
                CreatedAt = DateTime.Now,
                CustomerId = customer.Id,
                BranchId = request.BranchId,
                Price = city.Value.Price,
                OrderPrice = request.OrderPrice
            };
            _shippingDb.Orders.Add(order);
        }

        await _shippingDb.SaveChangesAsync(cancellationToken);
        return "تم انشاء الطلب بنجاح";
    }

    public async Task<Result<List<GetCustomerOrderResponse>>> GetOrderByCustomerAsync(GetOrderByCustomerRequest request,
        CancellationToken cancellationToken)
    {
        var customer =
            await _shippingDb.Customers.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId),
                cancellationToken);

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
                OrderPrice = x.OrderPrice,
                SenderPhoneNo = x.SenderPhoneNo,
                RecipientPhoneNo = x.RecipientPhoneNo,
                CreatedAt = x.CreatedAt.ToString("yyyy:MM:dd HH:mm:ss"),
                UpdatedAt = x.UpdatedAt.Value.ToString("yyyy:MM:dd HH:mm:ss"),
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

    public async Task<Result<List<GetRepresentativeOrderResponse>>> GetOrderByRepresentativeAsync(
        GetOrderByRepresentativeRequest request, CancellationToken cancellationToken)
    {
        var representatives =
            await _shippingDb.Representatives.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId),
                cancellationToken);

        var orders = await _shippingDb.Orders
            .Where(x => representatives != null && x.RepresentativesId == representatives.Id)
            .Select(x => new GetRepresentativeOrderResponse
            {
                OrderId = x.Id,
                OrderState = x.OrderState,
                OrderNo = x.OrderNo,
                RecipientAddress = x.RecipientAddress,
                Dscription = x.Dscription,
                Price = x.Price,
                OrderPrice = x.OrderPrice,
                RecipientPhoneNo = x.RecipientPhoneNo,
                SenderPhoneNo = x.SenderPhoneNo,
                CreatedAt = x.CreatedAt.ToString("yyyy:MM:dd HH:mm:ss"),
                UpdatedAt = x.UpdatedAt.Value.ToString("yyyy:MM:dd HH:mm:ss"),
            }).ToListAsync(cancellationToken);

        if (orders.Count <= 0)
            return Result.Fail("لايوجد طلبات");
        return orders;
    }

    public async Task<Result<string>> AcceptanceOrderAsync(AcceptanceOrdersRequest request,
        CancellationToken cancellationToken)
    {
        var employee =
            await _shippingDb.Employees.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId),
                cancellationToken);

        var order = await _shippingDb.Orders
            .FirstOrDefaultAsync(x => employee != null
                                      && x.OrderNo == request.OrderNo
                                      && x.BranchId == employee.BranchId, cancellationToken);
        if (order == null)
            return Result.Fail("الطلب غير موجود");

        order.OrderState = OrderState.InTheWarehouse;
        order.UpdatedAt = DateTime.Now;

        await _shippingDb.SaveChangesAsync(cancellationToken);
        return "تم قبول الطلب بنجاح";
    }

    public async Task<Result<string>> RollBackOrderAsync(RollBackOrderRequest request,
        CancellationToken cancellationToken)
    {
        var employee =
            await _shippingDb.Employees.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId),
                cancellationToken);

        var order = await _shippingDb.Orders
            .FirstOrDefaultAsync(x => employee != null
                                      && x.OrderNo == request.OrderNo
                                      && x.BranchId == employee.BranchId, cancellationToken);
        if (order == null)
            return Result.Fail("الطلب غير موجود");

        if (order.OrderState == OrderState.InTheWarehouse || order.OrderState == OrderState.ReturnInTheWarehouse)
            order.OrderState = OrderState.ReturnNdClosed;
        else if (order.OrderState == OrderState.Delivered)
            order.OrderState = OrderState.DeliveredNdClosed;
        else
            return Result.Fail("عذراً لا يمكن إغلاق الطلب");

        order.UpdatedAt = DateTime.Now;
        await _shippingDb.SaveChangesAsync(cancellationToken);
        return "تم إغلاق الطلب بنجاح";
    }

    public async Task<Result<string>> ChangeOrderStateByRepresentativeAsync(
        ChangeOrderStateByRepresentativeRequest request,
        CancellationToken cancellationToken)
    {
        var representatives =
            await _shippingDb.Representatives.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId),
                cancellationToken);

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
            OrderStateRepresentative.ReturnInTheWarehouse => OrderState.ReturnInTheWarehouse,
            _ => null
        };

        if (types is null)
            return Result.Fail("عذراً لا يمكن تغيير حالة الطلب");

        order.OrderState = types.Value;
        order.UpdatedAt = DateTime.Now;

        await _shippingDb.SaveChangesAsync(cancellationToken);
        return "تم تغيير حالة الطلب بنجاح";
    }

    public async Task<Result<string>> InsertRepresentativeInOrderAsync(InsertRepresentativeInOrderRequest request,
        CancellationToken cancellationToken)
    {
        var employee =
            await _shippingDb.Employees.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId),
                cancellationToken);

        var order = await _shippingDb.Orders
            .FirstOrDefaultAsync(x => employee != null
                                      && x.OrderNo == request.OrderNo
                                      && x.BranchId == employee.BranchId, cancellationToken);
        if (order == null)
            return Result.Fail("الطلب غير موجود");

        var result = await _shippingDb.Representatives
            .FirstOrDefaultAsync(x => x.UserId == request.RepresentativeId, cancellationToken);
        if (result == null)
            return Result.Fail("المندوب غير موجود");

        var representative = result.Id;
        order.RepresentativesId = representative;
        order.OrderState = OrderState.DeliveredToTheRepresentative;
        order.UpdatedAt = DateTime.Now;

        await _shippingDb.SaveChangesAsync(cancellationToken);
        return "تم تعيين المندوب بنجاح";
    }

    public async Task<Result<List<GetOrderResponse>>> GetOrderByBranchIdAsync(GetOrderByBranchIdRequest request,
        CancellationToken cancellationToken)
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
                OrderPrice = x.OrderPrice,
                CreatedAt = x.CreatedAt.ToString("yyyy:MM:dd HH:mm:ss"),
                UpdatedAt = x.UpdatedAt.Value.ToString("yyyy:MM:dd HH:mm:ss"),
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

    public async Task<Result<List<GetOrderResponse>>> GetOrderByOrderNoAsync(GetOrderByOrderNoRequest request,
        CancellationToken cancellationToken)
    {
        var employee =
            await _shippingDb.Employees.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId),
                cancellationToken);

        return await _sherdOrder.GetOrderByOrderNo(request.OrderNo, employee.BranchId.Value, cancellationToken);
    }

    public async Task<Result<List<GetOrderResponse>>> ShearchOrderAsync(ShearchOrderRequest request,
        CancellationToken cancellationToken)
    {
        var employee =
            await _shippingDb.Employees.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId),
                cancellationToken);

        var city = await _sherdOrder.GetCityId(employee.BranchId.Value, request.CityId, cancellationToken);

        OrderState? types = request.OrderState switch
        {
            OrderStateVm.Pending => OrderState.Pending,
            OrderStateVm.InTheWarehouse => OrderState.InTheWarehouse,
            OrderStateVm.DeliveredToTheRepresentative => OrderState.DeliveredToTheRepresentative,
            OrderStateVm.Delivered => OrderState.Delivered,
            OrderStateVm.Returning => OrderState.Returning,
            OrderStateVm.ReturnInTheWarehouse => OrderState.ReturnInTheWarehouse,
            OrderStateVm.DeliveredNdClosed => OrderState.DeliveredNdClosed,
            OrderStateVm.ReturnNdClosed => OrderState.ReturnNdClosed,
            _ => null
        };

        var orderCriteria = (types == null, request.CityId == Guid.Empty, request.RepresentativeId == Guid.Empty);

        return orderCriteria switch
        {
            (true, true, true) => await _sherdOrder.GetOrders(cancellationToken),
            (false, true, true) => await _sherdOrder.GetOrderByState(types.Value, employee.BranchId.Value,
                cancellationToken),
            (false, false, true) => await _sherdOrder.GetOrderByCityIdAndState(types.Value, city.Value.Name,
                employee.BranchId.Value,
                cancellationToken),
            (false, true, false) => await _sherdOrder.GetOrderByStateAndRepresentativeId(types.Value,
                request.RepresentativeId, employee.BranchId.Value, cancellationToken),
            (true, false, true) => await _sherdOrder.GetOrderByCityId(city.Value.Name, employee.BranchId.Value,
                cancellationToken),
            (true, false, false) => await _sherdOrder.GetOrderByCityIdAndRepresentativeId(request.RepresentativeId,
                city.Value.Name, employee.BranchId.Value, cancellationToken),
            (true, true, false) => await _sherdOrder.GetOrderByRepresentativeId(request.RepresentativeId,
                employee.BranchId.Value, cancellationToken),
            _ => await _sherdOrder.GetOrderByAll(types.Value, request.RepresentativeId, city.Value.Name,
                employee.BranchId.Value, cancellationToken)
        };
    }

    public async Task<Result<string>> DeleteOrder(DeleteOrderRequest request, CancellationToken cancellationToken)
    {
        var employee =
            await _shippingDb.Employees.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId),
                cancellationToken);

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

    public async Task<Result<string>> GetWallet(GetWalletRequest request, CancellationToken cancellationToken)
    {
        var customer =
            await _shippingDb.Customers.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId),
                cancellationToken);

        var total = await _shippingDb.Orders.Where(x =>
                customer != null && x.OrderState == OrderState.Delivered && x.CustomerId == customer.Id)
            .Select(x => x.OrderPrice).ToListAsync(cancellationToken);

        if (total.Count <= 0)
            return Result.Fail("لا يوجد رصيد في المحفظة");

        var totalPrice = total.Sum().ToString();

        if (totalPrice != null) return totalPrice;

        return "";
    }
}