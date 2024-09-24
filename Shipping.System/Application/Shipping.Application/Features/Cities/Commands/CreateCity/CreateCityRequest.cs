using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Cities.Commands.CreateCity;

public class CreateCityRequest : IRequest<Result<string>>
{
    public Guid BranchId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}