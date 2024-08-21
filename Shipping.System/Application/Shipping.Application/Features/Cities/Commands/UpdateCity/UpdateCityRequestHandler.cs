using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.Cities.Commands.UpdateCity;

public class UpdateCityRequestHandler : IRequestHandler<UpdateCityRequest, Result<string>>
{
    private readonly ICityRepository _cityRepository;

    public UpdateCityRequestHandler(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<Result<string>> Handle(UpdateCityRequest request, CancellationToken cancellationToken)
        => await _cityRepository.UpdateCityAsync(request, cancellationToken);
}