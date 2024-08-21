using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.Branchs.Commands.UpdateBranch;

public class UpdateBranchRequestHandler : IRequestHandler<UpdateBranchRequest, Result<string>>
{
    private readonly IBranchRepository _branchRepository;

    public UpdateBranchRequestHandler(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
    }

    public async Task<Result<string>> Handle(UpdateBranchRequest request, CancellationToken cancellationToken)
        => await _branchRepository.UpdateBranchAsync(request, cancellationToken);
}