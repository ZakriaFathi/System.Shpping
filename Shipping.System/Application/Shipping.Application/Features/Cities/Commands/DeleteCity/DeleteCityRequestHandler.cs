using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.Cities.Commands.DeleteCity;

public class DeleteCityRequestHandler : IRequestHandler<DeleteCityRequest, Result<string>>
{
    private readonly ICityRepository _cityRepository;

    public DeleteCityRequestHandler(ICityRepository cityRepository)
    {
        _cityRepository = cityRepository;
    }

    public async Task<Result<string>> Handle(DeleteCityRequest request, CancellationToken cancellationToken)
        => await _cityRepository.DeleteCity(request, cancellationToken);
}