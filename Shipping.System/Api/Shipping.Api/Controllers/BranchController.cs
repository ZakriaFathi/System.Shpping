using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shipping.Application.Features.Branchs.Commands.CreateBranch;
using Shipping.Application.Features.Branchs.Commands.UpdateBranch;
using Shipping.Application.Features.Branchs.Queries;
using Shipping.Application.Features.Branchs.Queries.GetBranchs;
using Shipping.Utils.Vm;

namespace Shipping.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BranchController : ControllerBase
{
    private readonly IMediator _mediator;

    public BranchController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("CreateBranch")]
    public async Task<OperationResult<string>> CreateBranch([FromBody] CreateBranchRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    }  
    [HttpPost("UpdateBranch")]
    public async Task<OperationResult<string>> UpdateBranch([FromBody] UpdateBranchRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    }
    [HttpGet("GetBranchs")]  
    public async Task<OperationResult<List<BranchsResopnse>>> GetBranchs(CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(new GetBranchsRequest(), cancellationToken);

        return result.ToOperationResult();
    }  
}