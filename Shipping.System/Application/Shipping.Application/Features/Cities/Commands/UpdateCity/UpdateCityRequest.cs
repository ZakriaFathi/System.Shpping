using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Cities.Commands.UpdateCity;

public class UpdateCityRequest : IRequest<Result<string>>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}