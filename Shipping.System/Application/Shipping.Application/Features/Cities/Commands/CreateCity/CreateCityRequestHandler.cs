using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.Cities.Commands.CreateCity;

public class CreateCityRequestHandler : IRequestHandler<CreateCityRequest, Result<string>>
{
    private readonly ICityRepository _cityRepository;

    public CreateCityRequestHandler(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<Result<string>> Handle(CreateCityRequest request, CancellationToken cancellationToken)
        => await _cityRepository.CreateCityAsync(request, cancellationToken);
}