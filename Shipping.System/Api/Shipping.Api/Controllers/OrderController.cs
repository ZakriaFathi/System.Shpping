using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shipping.Api.Models.Orders;
using Shipping.Api.Shared;
using Shipping.Application.Features.Orders.Commands.AcceptanceOrders;
using Shipping.Application.Features.Orders.Commands.ChangeOrderStateByEmployee;
using Shipping.Application.Features.Orders.Commands.ChangeOrderStateByRepresentative;
using Shipping.Application.Features.Orders.Commands.CreateOrder;
using Shipping.Application.Features.Orders.Commands.DeleteOrder;
using Shipping.Application.Features.Orders.Commands.InsertRepresentativeInOrder;
using Shipping.Application.Features.Orders.Queries;
using Shipping.Application.Features.Orders.Queries.GetOrderByBranchId;
using Shipping.Application.Features.Orders.Queries.GetOrderByCustomerId;
using Shipping.Application.Features.Orders.Queries.GetOrderByRepresentativeId;
using Shipping.Application.Features.Orders.Queries.ShearchOrder;
using Shipping.Utils.Vm;

namespace Shipping.Api.Controllers;

public class OrderController : BaseController
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("CreateOrder")]
    [Authorize(Roles = "User")]
    public async Task<OperationResult<string>> CreateOrder([FromQuery]Guid branchId,[FromQuery] Guid cityId,[FromQuery] string dscription,[FromQuery] int? countOfItems,[FromQuery] string senderPhoneNo,[FromQuery] string recipientPhoneNo, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateOrderRequest()
        {
            BranchId = branchId,
            CityId = cityId,
            Dscription = dscription,
            CountOfItems = countOfItems,
            SenderPhoneNo = senderPhoneNo,
            RecipientPhoneNo = recipientPhoneNo,
            UserId = GetUserId()
        }, cancellationToken);

        return result.ToOperationResult();
    }  
    
    [HttpPost("AcceptanceOrder")]
    [Authorize(Roles = "Employee")]
    public async Task<OperationResult<string>> AcceptanceOrder([FromBody] AcceptanceOrRejectOrdersVm request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AcceptanceOrdersRequest()
        {
            OrderNo = request.OrderNo,
            UserId = GetUserId()
        }, cancellationToken);

        return result.ToOperationResult();
    }  
    
    
    [HttpPost("ChangeOrderStateByEmployee")] 
    [Authorize(Roles = "Employee")]
    public async Task<OperationResult<string>> ChangeOrderStateByEmployee([FromBody] ChangeOrderStateByEmployeeVm request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ChangeOrderStateByEmployeeRequest()
        {
            OrderNo = request.OrderNo,
            OrderState = request.OrderState,
            UserId = GetUserId()
        }, cancellationToken);

        return result.ToOperationResult();
    } 
    
    [HttpPost("InsertRepresentativeInOrder")]
    [Authorize(Roles = "Employee")]
    public async Task<OperationResult<string>> InsertRepresentativeInOrder([FromBody] InsertRepresentativeInOrderVm request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new InsertRepresentativeInOrderRequest()
        {
            OrderNo = request.OrderNo,
            RepresentativeId = request.RepresentativeId,
            UserId = GetUserId()
        }, cancellationToken);

        return result.ToOperationResult();
    }  
   
    [HttpPost("ChangeOrderStateByRepresentative")]
    [Authorize(Roles = "Representative")]
    public async Task<OperationResult<string>> ChangeOrderStateByRepresentative([FromBody] ChangeOrderStateByRepresentativeVm request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ChangeOrderStateByRepresentativeRequest()
        {
            OrderNo = request.OrderNo,
            UserId = GetUserId()
        }, cancellationToken);

        return result.ToOperationResult();
    }
    
    [HttpGet("GetOrderByCustomer")] 
    [Authorize(Roles = "User")]
    public async Task<OperationResult<List<GetCustomerOrderResponse>>> GetOrderByCustomer(CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(new GetOrderByCustomerRequest()
        {
            UserId = GetUserId()
        }, cancellationToken);

        return result.ToOperationResult();
    } 
    
    [HttpGet("GetOrderByRepresentative")] 
    [Authorize(Roles = "Representative")]
    public async Task<OperationResult<List<GetRepresentativeOrderResponse>>> GetOrderByRepresentative(CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(new GetOrderByRepresentativeRequest()
        {
            UserId = GetUserId()
        }, cancellationToken);
        return result.ToOperationResult();
    } 
   
    [HttpGet("GetOrderByBranchId")]
    [Authorize(Roles = "Owner")]
    public async Task<OperationResult<List<GetOrderResponse>>> GetOrderByBranchId([FromQuery]GetOrderByBranchIdRequest request, CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    }
    [HttpGet("ShearchOrder")]
    [Authorize(Roles = "Employee")]
    public async Task<OperationResult<List<GetOrderResponse>>> ShearchOrder([FromQuery]ShearchOrderVm request, CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(new ShearchOrderRequest()
        {
            UserId = GetUserId(),
            OrderState = request.OrderState,
            CityId = request.CityId,
            RepresentativeId = request.RepresentativeId
        }, cancellationToken);

        return result.ToOperationResult();
    } 
    
    [HttpDelete("DeleteOrder")] 
    [Authorize(Roles = "Employee")]
    public async Task<OperationResult<string>> DeleteOrder([FromQuery]DeleteOrderVm request,CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(new DeleteOrderRequest()
        {
            Id = request.Id,
            UserId = GetUserId()
        }, cancellationToken);

        return result.ToOperationResult();
    } 
}