using FluentResults;
using Shipping.Application.Features.Cities.Commands.CreateCity;
using Shipping.Application.Features.Cities.Commands.DeleteCity;
using Shipping.Application.Features.Cities.Commands.UpdateCity;
using Shipping.Application.Features.Cities.Queries;
using Shipping.Application.Features.Cities.Queries.GetCities;
using Shipping.Application.Features.Cities.Queries.GetCitiesByBranchId;

namespace Shipping.Application.Abstracts;

public interface ICityRepository
{
    Task<Result<string>> CreateCityAsync(CreateCityRequest request, CancellationToken cancellationToken);
    Task<Result<string>> UpdateCityAsync(UpdateCityRequest request, CancellationToken cancellationToken);
    Task<Result<List<CitiesResopnse>>> GetCitiesAsync(GetCitiesRequest request, CancellationToken cancellationToken);
    Task<Result<List<CitiesResopnse>>> GetCitiesByBranchIdAsync(GetCitiesByBranchIdRequest request, CancellationToken cancellationToken);
    Task<Result<string>> DeleteCity(DeleteCityRequest request, CancellationToken cancellationToken);


}