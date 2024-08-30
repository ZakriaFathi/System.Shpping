using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Cities.Queries.GetCities;

public class GetCitiesRequest : IRequest<Result<List<CitiesResopnse>>>
{
    public string UserId { get; set; }

}