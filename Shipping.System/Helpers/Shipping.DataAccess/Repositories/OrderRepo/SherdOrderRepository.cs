using FluentResults;
using Microsoft.EntityFrameworkCore;
using Shipping.Application.Abstracts;
using Shipping.Application.Features.Branchs.Queries.GetBranchById;
using Shipping.Application.Features.Cities.Queries.GetCitiesByBranchId;
using Shipping.Application.Features.Orders.Queries;
using Shipping.Application.Models;
using Shipping.DataAccess.Persistence.DataBase;
using Shipping.Utils.Enums;

namespace Shipping.DataAccess.Repositories.OrderRepo;

public class SherdOrderRepository : ISherdOrderRepository
{
    private readonly ShippingDbContext _shippingDb;
    private readonly ICityRepository _cityRepository;
    private readonly IBranchRepository _branchRepository;

    public SherdOrderRepository(ShippingDbContext shippingDb, ICityRepository cityRepository, IBranchRepository branchRepository)
    {
        _shippingDb = shippingDb;
        _cityRepository = cityRepository;
        _branchRepository = branchRepository;
    }

    public async Task<Result<List<GetOrderResponse>>> GetOrders(CancellationToken cancellationToken)
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
                OrderPrice = x.OrderPrice,
                CreatedAt = x.CreatedAt.ToString("yyyy:MM:dd HH:mm:ss"),
                UpdatedAt = x.UpdatedAt.Value.ToString("yyyy:MM:dd HH:mm:ss"),
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
        if (orders.Count <= 0)
            return Result.Fail<List<GetOrderResponse>>("لا يوجد طلبات");
        return orders;
    }

    public async Task<Result<CityVm>> GetCityId(Guid branchId, Guid cityId, CancellationToken cancellationToken)
    {
        var branch = await _branchRepository.GetBranchByIdAsync(new GetBranchByIdRequest()
        {
            BranchId = branchId
        }, cancellationToken);
        if (!branch.IsSuccess)
            return Result.Fail(branch.Errors.ToList());
        
        var allCities = await _cityRepository.GetCitiesByBranchIdAsync(new GetCitiesByBranchIdRequest()
        {
            BranchId = branch.Value.BranchId
        }, cancellationToken);
        if (!allCities.IsSuccess)
            return Result.Fail(allCities.Errors.ToList());

        var city = allCities.Value.SelectMany(x => x.Cities)
            .FirstOrDefault(x => x.CityId == cityId);
        
        if (city is null)
            return Result.Fail<CityVm>("المدينة غير موجودة");
        
        var cityVm = new CityVm
        {
            Name = city.Name,
            Price = city.Price.Value
        };
        
        return cityVm;
    }

    public async Task<Result<List<GetOrderResponse>>> GetOrderByState(OrderState state, Guid branchId, CancellationToken cancellationToken)
    {
        var orders = await _shippingDb.Orders
            .Where(x => x.BranchId == branchId &&
                        x.OrderState == state 
            )
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

        if (orders.Count <= 0)
            return Result.Fail("لا يوجد طلبات");

        return orders;
    }

    public async Task<Result<List<GetOrderResponse>>> GetOrderByStateAndRepresentativeId(OrderState state, Guid representativeId, Guid branchId,
        CancellationToken cancellationToken)
    {
        var representative = await _shippingDb.Representatives.FirstOrDefaultAsync(x => x.UserId == representativeId, cancellationToken);
        if(representative == null)
            return Result.Fail("الرجاء تأكد من رقم المندوب"); 
        
        var orders = await _shippingDb.Orders
            .Where(x => x.BranchId == branchId &&
                        x.OrderState == state &&
                        x.RepresentativesId == representative.Id
            )
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

        if (orders.Count <= 0)
            return Result.Fail("لا يوجد طلبات");

        return orders;
    }

    public async Task<Result<List<GetOrderResponse>>> GetOrderByRepresentativeId(Guid representativeId, Guid branchId, CancellationToken cancellationToken)
    {
        var representative = await _shippingDb.Representatives.FirstOrDefaultAsync(x => x.UserId == representativeId, cancellationToken);
        if(representative == null)
            return Result.Fail("الرجاء تأكد من رقم المندوب"); 
        
        var orders = await _shippingDb.Orders
            .Where(x => x.BranchId == branchId &&
                        x.RepresentativesId == representative.Id )
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

        if (orders.Count <= 0)
            return Result.Fail("لا يوجد طلبات");

        return orders;
    }

    public async Task<Result<List<GetOrderResponse>>> GetOrderByCityIdAndState(OrderState state, string cityName, Guid branchId, CancellationToken cancellationToken)
    {
        var orders = await _shippingDb.Orders
            .Where(x => x.BranchId == branchId &&
                        x.OrderState == state &&
                        x.RecipientAddress == cityName
            )
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

        if (orders.Count <= 0)
            return Result.Fail("لا يوجد طلبات");

        return orders;
    }

    public async Task<Result<List<GetOrderResponse>>> GetOrderByCityId(string cityName, Guid branchId, CancellationToken cancellationToken)
    {
        var orders = await _shippingDb.Orders
            .Where(x => x.BranchId == branchId &&
                        x.RecipientAddress == cityName
            )
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

        if (orders.Count <= 0)
            return Result.Fail("لا يوجد طلبات");

        return orders;
    }

    public async Task<Result<List<GetOrderResponse>>> GetOrderByCityIdAndRepresentativeId(Guid representativeId, string cityName, Guid branchId,
        CancellationToken cancellationToken)
    {
        var representative = await _shippingDb.Representatives.FirstOrDefaultAsync(x => x.UserId == representativeId, cancellationToken);
        if(representative == null)
            return Result.Fail("الرجاء تأكد من رقم المندوب"); 
        
        var orders = await _shippingDb.Orders
            .Where(x => x.BranchId == branchId &&
                        x.RecipientAddress == cityName &&
                        x.RepresentativesId == representative.Id 
            )
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

        if (orders.Count <= 0)
            return Result.Fail("لا يوجد طلبات");

        return orders;
    }

    public async Task<Result<List<GetOrderResponse>>> GetOrderBySenderPhoneNo(string senderPhoneNo, Guid branchId,
        CancellationToken cancellationToken)
    {
        var orders = await _shippingDb.Orders.Where(x => x.SenderPhoneNo == senderPhoneNo)
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
        if (orders.Count <= 0)
            return Result.Fail("لا يوجد طلبات");

        return orders;
    }

    public async Task<Result<List<GetOrderResponse>>> GetOrderBySenderPhoneNoAndOrderNo(string senderPhoneNo, string orderNo, Guid branchId,
        CancellationToken cancellationToken)
    {
        var orders = await _shippingDb.Orders
            .Where(x => x.SenderPhoneNo == senderPhoneNo &&
                        x.OrderNo == orderNo)
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
        if (orders.Count <= 0)
            return Result.Fail("لا يوجد طلبات");

        return orders;
    }

    public async Task<Result<List<GetOrderResponse>>> GetOrderByOrderNo(string orderNo, Guid branchId, CancellationToken cancellationToken)
    {
        var orders = await _shippingDb.Orders.Where(x => x.OrderNo == orderNo)
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
        if (orders.Count <= 0)
            return Result.Fail("لا يوجد طلبات");

        return orders;    
    }

    public async Task<Result<List<GetOrderResponse>>> GetOrderByAll(OrderState state, Guid representativeId, string cityName, Guid branchId,
        CancellationToken cancellationToken)
    {
        var orders = await _shippingDb.Orders
            .Where(x => x.BranchId == branchId &&
                        x.OrderState == state &&
                        x.RecipientAddress == cityName &&
                        x.RepresentativesId == representativeId
            )
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
        
        if (orders.Count <= 0)
            return Result.Fail("لا يوجد طلبات");

        return orders;
    }
    
    

}