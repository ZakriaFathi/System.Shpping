using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Cities.Queries.GetCitiesByBranchId;

public class GetCitiesByBranchIdRequest : IRequest<Result<List<CitiesResopnse>>>
{
    public Guid BranchId { get; set; }
}