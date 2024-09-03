using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shipping.Application.Features.UserManagement.Permissions;
using Shipping.Application.Features.UserManagement.Permissions.Commands.DeletePermission;
using Shipping.Application.Features.UserManagement.Permissions.Queries.GetAllPermissions;
using Shipping.Application.Features.UserManagement.Permissions.Queries.GetPermissionsByRoleId;
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
    [Authorize("PermissionManagementView")]
    public async Task<OperationResult<List<GetPermissionsResponse>>> GetAllPermissions(CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(new GetAllPermissionsRequest(), cancellationToken);

        return result.ToOperationResult();
    } 
    [HttpGet("GetAllPermissionsByUserId")] 
    [Authorize("PermissionManagementView")]
    public async Task<OperationResult<List<GetPermissionsResponse>>> GetAllPermissionsByUserId([FromQuery]GetAllPermissionsByUserIdRequest request,CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    }
    [HttpDelete("DeleteUserPermissions")] 
    [Authorize("PermissionManagementDelete")]
    public async Task<OperationResult<string>> DeleteUserPermissions([FromQuery]DeletePermissionRequest request,CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    } 
}