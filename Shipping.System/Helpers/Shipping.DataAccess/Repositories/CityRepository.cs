using FluentResults;
using Microsoft.EntityFrameworkCore;
using Shipping.Application.Abstracts;
using Shipping.Application.Features.Cities.Commands.CreateCity;
using Shipping.Application.Features.Cities.Commands.UpdateCity;
using Shipping.Application.Features.Cities.Queries;
using Shipping.Application.Features.Cities.Queries.GetCities;
using Shipping.Application.Features.Cities.Queries.GetCitiesByBranchId;
using Shipping.DataAccess.Persistence.DataBase;
using Shipping.Domain.Entities;

namespace Shipping.DataAccess.Repositories;

public class CityRepository : ICityRepository
{
    private readonly ShippingDbContext _shippingDb;

    public CityRepository(ShippingDbContext shippingDb)
    {
        _shippingDb = shippingDb;
    }

    public async Task<Result<string>> CreateCityAsync(CreateCityRequest request, CancellationToken cancellationToken)
    {
        var city = await _shippingDb.Cities.FirstOrDefaultAsync(x => x.Name == request.Name, cancellationToken);
        if (city != null)
            return Result.Fail("المدينة موجودة مسبقا");
        
        var newCity = new City
        {
            Name = request.Name,
            Price = request.Price,
            BranchId = request.BranchId
        };
        
        await _shippingDb.Cities.AddAsync(newCity, cancellationToken);
        await _shippingDb.SaveChangesAsync(cancellationToken);
        
        return Result.Ok("تمت عملية الاضافة بنجاح");
        
    }

    public async Task<Result<string>> UpdateCityAsync(UpdateCityRequest request, CancellationToken cancellationToken)
    {
        var city = await _shippingDb.Cities.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (city == null)
            return Result.Fail("المدينة غير موجودة مسبقا");    
        
        city.Name = request.Name;
        city.Price = request.Price;
        
        await _shippingDb.SaveChangesAsync(cancellationToken);
        
        return Result.Ok("تمت عملية التعديل بنجاح");
        
    }

    public async Task<Result<List<CitiesResopnse>>> GetCitiesAsync(GetCitiesRequest request, CancellationToken cancellationToken)
    {
        var cities = await _shippingDb.Cities
            .Include(p=>p.Branch)
            .GroupBy(u => u.Branch)
            .Select(g => new CitiesResopnse()
            {
                BranchId = g.First(x=>x.BranchId == x.Branch.Id).Branch.Id,
                Cities = g.Select(x => new Cities
                {
                    CityId = x.Id,
                    Name = x.Name,
                    Price = x.Price
            }).ToList()
            }).ToListAsync(cancellationToken);
        
         
        if (cities.Count <= 0)
            return Result.Fail<List<CitiesResopnse>>( "لا يوجد مدن" );

        return cities;
    }

    public async Task<Result<List<CitiesResopnse>>> GetCitiesByBranchIdAsync(GetCitiesByBranchIdRequest request, CancellationToken cancellationToken)
    {
        var cities = await _shippingDb.Cities
            .Include(p=>p.Branch)
            .Where(x => x.BranchId == request.BranchId)
            .GroupBy(u => u.Branch)
            .Select(g => new CitiesResopnse()
            {
                BranchId = g.First(x=>x.BranchId == x.Branch.Id).Branch.Id,
                Cities = g.Select(x => new Cities
                {
                    CityId = x.Id,
                    Name = x.Name,
                    Price = x.Price
                }).ToList()
            }).ToListAsync(cancellationToken);
        
         
        if (cities.Count <= 0)
            return Result.Fail<List<CitiesResopnse>>( "لا يوجد مدن" );

        return cities;
    }
}