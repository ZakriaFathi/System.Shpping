using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;
using Shipping.Application.Features.Cities.Queries.GetCitiesByBranchId;

namespace Shipping.Application.Features.Cities.Queries.GetCities;

public class GetCitiesRequestHandler : IRequestHandler<GetCitiesRequest,  Result<List<CitiesResopnse>>>
{
    private readonly ICityRepository _cityRepository;

    public GetCitiesRequestHandler(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }
    public async Task<Result<List<CitiesResopnse>>> Handle(GetCitiesRequest request, CancellationToken cancellationToken)
        => await _cityRepository.GetCitiesAsync(request, cancellationToken);
}