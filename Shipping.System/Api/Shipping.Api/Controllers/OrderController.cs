using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shipping.Application.Features.Orders.Commands.AcceptanceOrders;
using Shipping.Application.Features.Orders.Commands.ChangeOrderStateByEmployee;
using Shipping.Application.Features.Orders.Commands.ChangeOrderStateByRepresentative;
using Shipping.Application.Features.Orders.Commands.CreateOrder;
using Shipping.Application.Features.Orders.Commands.DeleteOrder;
using Shipping.Application.Features.Orders.Commands.InsertRepresentativeInOrder;
using Shipping.Application.Features.Orders.Commands.ToRejectOrder;
using Shipping.Application.Features.Orders.Queries.GetOrderByBranchId;
using Shipping.Application.Features.Orders.Queries.GetOrderByCustomerId;
using Shipping.Application.Features.Orders.Queries.GetOrderByRepresentativeId;
using Shipping.Application.Features.Orders.Queries.GetOrders;
using Shipping.Utils.Vm;

namespace Shipping.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("CreateOrder")]
    public async Task<OperationResult<string>> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    }  
    [HttpPost("AcceptanceOrder")]
    public async Task<OperationResult<string>> AcceptanceOrder([FromBody] AcceptanceOrdersRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    }  
    [HttpPost("ToRejectOrder")]
    public async Task<OperationResult<string>> ToRejectOrder([FromBody] ToRejectOrderRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    } 
    [HttpPost("ChangeOrderStateByEmployee")] 
    public async Task<OperationResult<string>> ChangeOrderStateByAdmin([FromBody] ChangeOrderStateByEmployeeRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    } 
    [HttpPost("InsertRepresentativeInOrder")]
    public async Task<OperationResult<string>> InsertRepresentativeInOrder([FromBody] InsertRepresentativeInOrderRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    }  
    [HttpPost("ChangeOrderStateByRepresentative")]
    public async Task<OperationResult<string>> ChangeOrderStateByRepresentative([FromBody] ChangeOrderStateByRepresentativeRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    }
    
    [HttpGet("GetOrderByCustomerId")] 
    public async Task<OperationResult<List<GetCustomerOrderResponse>>> GetOrderByCustomerId([FromQuery]GetOrderByCustomerIdRequest request, CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    } 
    [HttpGet("GetOrderByRepresentativeId")] 
    public async Task<OperationResult<List<GetRepresentativeOrderResponse>>> GetOrderByRepresentativeId([FromQuery]GetOrderByRepresentativeIdRequest request, CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    } 
    
    [HttpGet("GetOrders")] 
    public async Task<OperationResult<List<GetOrderResponse>>> GetOrders(CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(new GetOrdersRequest(), cancellationToken);

        return result.ToOperationResult();
    }  
    [HttpGet("GetOrderByBranchId")]  
    public async Task<OperationResult<List<GetOrderResponse>>> GetOrderByBranchId([FromQuery]GetOrderByBranchIdRequest request, CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    } 
    
    [HttpDelete("DeleteOrder")]  
    public async Task<OperationResult<string>> DeleteOrder([FromQuery]DeleteOrderRequest request,CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    } 
}