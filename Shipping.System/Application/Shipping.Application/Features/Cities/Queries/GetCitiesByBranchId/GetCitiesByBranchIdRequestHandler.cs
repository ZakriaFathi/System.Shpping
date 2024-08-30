using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.Cities.Queries.GetCitiesByBranchId;

public class GetCitiesByBranchIdRequestHandler : IRequestHandler<GetCitiesByBranchIdRequest, Result<List<CitiesResopnse>>>
{
    private readonly ICityRepository _cityRepository;

    public GetCitiesByBranchIdRequestHandler(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }
    public async Task<Result<List<CitiesResopnse>>> Handle(GetCitiesByBranchIdRequest request, CancellationToken cancellationToken)
    => await _cityRepository.GetCitiesByBranchIdAsync(request, cancellationToken);
}