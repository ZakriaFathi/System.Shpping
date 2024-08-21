using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Branchs.Queries.GetBranchById;

public class GetBranchByIdRequestHandler : IRequestHandler<GetBranchByIdRequest, Result<BranchsResopnse>>
{
    public async Task<Result<BranchsResopnse>> Handle(GetBranchByIdRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}