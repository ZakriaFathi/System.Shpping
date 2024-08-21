using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.Branchs.Queries.GetBranchs;

public class GetBranchsRequestHandler : IRequestHandler<GetBranchsRequest, Result<List<BranchsResopnse>>>
{
    private readonly IBranchRepository _branchRepository;

    public GetBranchsRequestHandler(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
    }

    public async Task<Result<List<BranchsResopnse>>> Handle(GetBranchsRequest request,
        CancellationToken cancellationToken)
        => await _branchRepository.GetBranchsAsync(request, cancellationToken);
}