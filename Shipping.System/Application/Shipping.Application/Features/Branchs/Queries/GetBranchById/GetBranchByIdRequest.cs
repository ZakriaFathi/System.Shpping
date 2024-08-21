using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Branchs.Queries.GetBranchById;

public class GetBranchByIdRequest : IRequest<Result<BranchsResopnse>>
{
    public Guid BranchId { get; set; }
}