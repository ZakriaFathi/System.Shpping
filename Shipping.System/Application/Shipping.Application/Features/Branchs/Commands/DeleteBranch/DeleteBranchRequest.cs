using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Branchs.Commands.DeleteBranch;

public class DeleteBranchRequest : IRequest<Result<string>>
{
    public Guid Id { get; set; }

}