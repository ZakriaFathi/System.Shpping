using FluentResults;
using Shipping.Application.Features.Branchs.Commands.CreateBranch;
using Shipping.Application.Features.Branchs.Commands.UpdateBranch;
using Shipping.Application.Features.Branchs.Queries;
using Shipping.Application.Features.Branchs.Queries.GetBranchById;
using Shipping.Application.Features.Branchs.Queries.GetBranchs;

namespace Shipping.Application.Abstracts;

public interface IBranchRepository
{
    Task<Result<string>> CreateBranchAsync(CreateBranchRequest request, CancellationToken cancellationToken);
    Task<Result<string>> UpdateBranchAsync(UpdateBranchRequest request, CancellationToken cancellationToken);
    Task<Result<List<BranchsResopnse>>> GetBranchsAsync(GetBranchsRequest request, CancellationToken cancellationToken);
    Task<Result<BranchsResopnse>> GetBranchByIdAsync(GetBranchByIdRequest request, CancellationToken cancellationToken);
}