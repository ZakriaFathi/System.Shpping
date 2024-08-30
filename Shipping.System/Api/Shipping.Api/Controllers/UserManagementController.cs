using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shipping.Api.Shared;
using Shipping.Application.Features.UserManagement.Users.Commands.ChangeUserActivation;
using Shipping.Application.Features.UserManagement.Users.Commands.CreateUser;
using Shipping.Application.Features.UserManagement.Users.Commands.CreateUserPermissions;
using Shipping.Application.Features.UserManagement.Users.Commands.DeleteUser;
using Shipping.Application.Features.UserManagement.Users.Commands.ResetPassword;
using Shipping.Application.Features.UserManagement.Users.Commands.UpdateUser;
using Shipping.Application.Features.UserManagement.Users.Commands.UpdateUserPermissions;
using Shipping.Application.Features.UserManagement.Users.Queries;
using Shipping.Application.Features.UserManagement.Users.Queries.GetAdmins;
using Shipping.Application.Features.UserManagement.Users.Queries.GetAdminsByBranchId;
using Shipping.Application.Features.UserManagement.Users.Queries.GetCustomers;
using Shipping.Application.Features.UserManagement.Users.Queries.GetRepresentatives;
using Shipping.Application.Features.UserManagement.Users.Queries.GetRepresentativesByBranchId;
using Shipping.Utils.Vm;

namespace Shipping.Api.Controllers;

public class UserManagementController : BaseController
{
    private readonly IMediator _mediator;

    public UserManagementController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("GetCustomers")]  
    [Authorize(Roles = "Owner")]
    public async Task<OperationResult<List<GetCustomersResponse>>> GetCustomers(CancellationToken cancellationToken)
    { 
       var result = await _mediator.Send(new GetCustomersRequest(), cancellationToken);

       return result.ToOperationResult();
    }
    [HttpGet("GetRepresentatives")]  
    [Authorize(Roles = "Employee")]
    public async Task<OperationResult<List<GetUsersResponse>>> GetRepresentatives(CancellationToken cancellationToken)
    { 
       var result = await _mediator.Send(new GetRepresentativesRequest()
       {
           UserId = GetUserId()
       }, cancellationToken);

       return result.ToOperationResult();
    } 
    [HttpGet("GetRepresentativesByBranchId")] 
    [Authorize(Roles = "Employee")]
    public async Task<OperationResult<List<GetUsersResponse>>> GetRepresentativesByBranchId([FromQuery]GetRepresentativesByBranchIdRequest request,CancellationToken cancellationToken)
    { 
       var result = await _mediator.Send(request, cancellationToken);

       return result.ToOperationResult();
    } 
    [HttpGet("GetEmployees")]  
    [Authorize(Roles = "Employee")]
    public async Task<OperationResult<List<GetUsersResponse>>> GetEmployees(CancellationToken cancellationToken)
    { 
       var result = await _mediator.Send(new GetEmployeesRequest()
       {
           UserId = GetUserId()
       }, cancellationToken);

       return result.ToOperationResult();
    }
    [HttpGet("GetEmployeesByBranchId")] 
    [Authorize(Roles = "Employee")]
    public async Task<OperationResult<List<GetUsersResponse>>> GetEmployeesByBranchId([FromQuery]GetEmployeesByBranchIdRequest request,CancellationToken cancellationToken)
    { 
       var result = await _mediator.Send(request, cancellationToken);

       return result.ToOperationResult();
    }
    
    [HttpPost("CreateUser")]
    public async Task<OperationResult<string>> CreateUser([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
        return result.ToOperationResult();
    }
    
    [HttpPost("ChangeUserActivation")]
    [Authorize(Roles = "Employee")]
    public async Task<OperationResult<string>> ChangeUserActivation([FromBody] ChangeUserActivationRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
        return result.ToOperationResult();
    }
    
    [HttpPost("ResetPassword")]
    [Authorize(Roles = "Employee")]
    public async Task<OperationResult<string>> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
        return result.ToOperationResult();
    } 
    [HttpPost("CreateUserPermissions")]
    
    public async Task<OperationResult<string>> CreateUserPermissions([FromBody] CreateUserPermissionsRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
        return result.ToOperationResult();
    }
    [HttpPost("UpdateUserPermissions")]
    public async Task<OperationResult<string>> UpdateUserPermissions([FromBody] UpdateUserPermissionsRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
        return result.ToOperationResult();
    }
    [HttpPost("UpdateUser")]
    public async Task<OperationResult<string>> UpdateUser([FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        
        return result.ToOperationResult();
    }
    
    [HttpDelete("DeleteUser")]  
    public async Task<OperationResult<string>> DeleteUser([FromQuery]DeleteUserRequest request,CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    } 
}