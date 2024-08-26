using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.Branchs.Commands.DeleteBranch;

public class DeleteBranchRequestHandler : IRequestHandler<DeleteBranchRequest, Result<string>>
{
    private readonly IBranchRepository _branchRepository;

    public DeleteBranchRequestHandler(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
    }

    public async Task<Result<string>> Handle(DeleteBranchRequest request, CancellationToken cancellationToken)
        => await _branchRepository.DeleteBranch(request, cancellationToken);
}