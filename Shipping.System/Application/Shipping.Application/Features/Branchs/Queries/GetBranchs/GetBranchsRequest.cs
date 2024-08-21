using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Branchs.Queries.GetBranchs;

public class GetBranchsRequest : IRequest<Result<List<BranchsResopnse>>>
{
    
}