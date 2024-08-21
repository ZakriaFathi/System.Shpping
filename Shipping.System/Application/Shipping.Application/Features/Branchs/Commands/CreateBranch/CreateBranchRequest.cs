using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Branchs.Commands.CreateBranch;

public class CreateBranchRequest : IRequest<Result<string>>
{
    public string Name { get; set; }
    public bool IsMajor { get; set; }
}