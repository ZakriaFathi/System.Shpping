using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Branchs.Commands.UpdateBranch;

public class UpdateBranchRequest : IRequest<Result<string>>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsMajor { get; set; }
}