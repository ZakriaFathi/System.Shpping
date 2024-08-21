using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shipping.Application.Features.Orders.Commands.CreateOrder;
using Shipping.Application.Features.Orders.Queries.GetOrderByCustomerId;
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
    
    [HttpGet("GetOrderByCustomerId")] 
    public async Task<OperationResult<List<GetCustomerOrderResponse>>> GetOrderByCustomerId([FromQuery]GetOrderByCustomerIdRequest request, CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    } 
}