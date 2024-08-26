using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Cities.Commands.DeleteCity;

public class DeleteCityRequest : IRequest<Result<string>>
{
    public Guid Id { get; set; }

}