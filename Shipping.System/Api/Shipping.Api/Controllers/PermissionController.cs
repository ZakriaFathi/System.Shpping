using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shipping.Application.Features.UserManagement.Permissions;
using Shipping.Application.Features.UserManagement.Permissions.Commands.CreatePermission;
using Shipping.Application.Features.UserManagement.Permissions.Commands.DeletePermission;
using Shipping.Application.Features.UserManagement.Permissions.Queries.GetAllPermissions;
using Shipping.Application.Features.UserManagement.Permissions.Queries.GetPermissionsByRoleId;
using Shipping.Application.Features.UserManagement.Users.Queries;
using Shipping.Application.Features.UserManagement.Users.Queries.GetAdminsByBranchId;
using Shipping.Utils.Vm;

namespace Shipping.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PermissionController : ControllerBase
{
    private readonly IMediator _mediator;

    public PermissionController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("GetAllPermissions")]  
    public async Task<OperationResult<List<GetPermissionsResponse>>> GetAllPermissions(CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(new GetAllPermissionsRequest(), cancellationToken);

        return result.ToOperationResult();
    } 
    [HttpGet("GetAllPermissionsByRoleId")]  
    public async Task<OperationResult<List<GetPermissionsResponse>>> GetAllPermissionsByRoleId(GetAllPermissionsByRoleIdRequest request,CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    }
    
    [HttpPost("CreatePermission")]
    public async Task<OperationResult<string>> CreatePermission([FromBody] CreatePermissionRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
        return result.ToOperationResult();
    }
    [HttpDelete("DeletePermission")]  
    public async Task<OperationResult<string>> DeletePermission([FromQuery]DeletePermissionRequest request,CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    } 
}