using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Cities.Commands.CreateCity;

public class CreateCityRequest : IRequest<Result<string>>
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string UserId { get; set; }
}