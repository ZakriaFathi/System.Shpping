using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shipping.Application.Features.Auth.Commands.SingIn;
using Shipping.Application.Features.Auth.Commands.SingUp;
using Shipping.Utils.Vm;

namespace Shipping.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("SingUp")]
    public async Task<OperationResult<string>> SingUp([FromBody] SingUpRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    }
    
    [HttpPost("SingIn")]
    public async Task<OperationResult<SingInResponse>> SingIn([FromQuery] SingInRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
        return result.ToOperationResult();
    }
}