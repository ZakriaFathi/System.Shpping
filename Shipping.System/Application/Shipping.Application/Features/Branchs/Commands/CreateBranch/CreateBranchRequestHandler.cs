using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.Branchs.Commands.CreateBranch;

public class CreateBranchRequestHandler : IRequestHandler<CreateBranchRequest, Result<string>>
{
    private readonly IBranchRepository _branchRepository;

    public CreateBranchRequestHandler(IBranchRepository branchRepository)
    {
        _branchRepository = branchRepository;
    }

    public async Task<Result<string>> Handle(CreateBranchRequest request, CancellationToken cancellationToken)
        => await _branchRepository.CreateBranchAsync(request, cancellationToken);
}