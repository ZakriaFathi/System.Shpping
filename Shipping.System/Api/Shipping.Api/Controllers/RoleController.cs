using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shipping.Application.Features.UserManagement.Roles;
using Shipping.Application.Features.UserManagement.Roles.Commands.CreateRole;
using Shipping.Application.Features.UserManagement.Roles.Queries.GetAllRoles;
using Shipping.Application.Features.UserManagement.Roles.Queries.GetRolesByUserId;
using Shipping.Utils.Vm;

namespace Shipping.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoleController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("GetAllRoles")]  
    public async Task<OperationResult<List<GetRolesResponse>>> GetAllRoles(CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(new GetAllRolesRequest(), cancellationToken);

        return result.ToOperationResult();
    } 
    [HttpGet("GetRolesByUserId")]  
    public async Task<OperationResult<List<GetRolesResponse>>> GetRolesByUserId([FromQuery] GetRolesByUserIdRequest request, CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    }
    
    [HttpPost("CreateRole")]
    public async Task<OperationResult<string>> CreateRole([FromBody] CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
        return result.ToOperationResult();
    }
}