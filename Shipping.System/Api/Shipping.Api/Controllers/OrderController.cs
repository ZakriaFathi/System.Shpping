using System.ComponentModel.DataAnnotations;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shipping.Api.Models.Orders;
using Shipping.Api.Shared;
using Shipping.Application.Features.Orders.Commands.AcceptanceOrders;
using Shipping.Application.Features.Orders.Commands.ChangeOrderStateByRepresentative;
using Shipping.Application.Features.Orders.Commands.CreateOrder;
using Shipping.Application.Features.Orders.Commands.DeleteOrder;
using Shipping.Application.Features.Orders.Commands.InsertRepresentativeInOrder;
using Shipping.Application.Features.Orders.Commands.RollBackOrder;
using Shipping.Application.Features.Orders.Queries;
using Shipping.Application.Features.Orders.Queries.GetOrderByOrderNo;
using Shipping.Application.Features.Orders.Queries.GetOrderByBranchId;
using Shipping.Application.Features.Orders.Queries.GetOrderByCustomerId;
using Shipping.Application.Features.Orders.Queries.GetOrderByRepresentativeId;
using Shipping.Application.Features.Orders.Queries.GetWallet;
using Shipping.Application.Features.Orders.Queries.ShearchOrder;
using Shipping.Utils.Vm;
using Swashbuckle.AspNetCore.Annotations;

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
    public async Task<OperationResult<string>> CreateOrder([FromQuery]Guid branchId,[FromQuery] Guid cityId,[FromQuery] string dscription,[FromQuery] int? countOfItems,[FromQuery] decimal orderPrice , [FromQuery] string recipientPhoneNo, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateOrderRequest()
        {
            BranchId = branchId,
            CityId = cityId,
            Dscription = dscription,
            CountOfItems = countOfItems,
            OrderPrice = orderPrice,
            RecipientPhoneNo = recipientPhoneNo,
            UserId = GetUserId()
        }, cancellationToken);

        return result.ToOperationResult();
    }  
    
    [HttpPost("AcceptanceOrder")]
    [Authorize("OrderManagementEdit")]
    public async Task<OperationResult<string>> AcceptanceOrder([FromBody] AcceptanceOrRollBackOrdersVm request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new AcceptanceOrdersRequest()
        {
            OrderNo = request.OrderNo,
            UserId = GetUserId()
        }, cancellationToken);

        return result.ToOperationResult();
    }
    
    [HttpPost("RollBackOrder")]
    [Authorize("OrderManagementEdit")]
    public async Task<OperationResult<string>> RollBackOrder([FromBody] AcceptanceOrRollBackOrdersVm request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new RollBackOrderRequest()
        {
            OrderNo = request.OrderNo,
            UserId = GetUserId()
        }, cancellationToken);

        return result.ToOperationResult();
    }  
    
    
    [HttpPost("InsertRepresentativeInOrder")]
    [Authorize("OrderManagementEdit")]
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
            OrderState = request.OrderState,
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
    [Authorize("OrderManagementView")]
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
    [HttpGet("GetOrderByOrderNo")]
    [Authorize("OrderManagementView")]
    public async Task<OperationResult<List<GetOrderResponse>>> GetOrderByOrderNo([FromQuery]GetOrderByOrderNoVm request , CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(new GetOrderByOrderNoRequest()
        {
            UserId = GetUserId(),
            OrderNo = request.OrderNo,
        }, cancellationToken);

        return result.ToOperationResult();
    } 
    
    [HttpDelete("DeleteOrder")] 
    [Authorize("OrderManagementDelete")]
    public async Task<OperationResult<string>> DeleteOrder([FromQuery]DeleteOrderVm request,CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(new DeleteOrderRequest()
        {
            Id = request.Id,
            UserId = GetUserId()
        }, cancellationToken);

        return result.ToOperationResult();
    } 
    
    [HttpGet("GetWallet")] 
    [Authorize(Roles = "User")]
    public async Task<OperationResult<string>> GetWallet(CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(new GetWalletRequest()
        {
            UserId = GetUserId()
        }, cancellationToken);

        return result.ToOperationResult();
    } 
}