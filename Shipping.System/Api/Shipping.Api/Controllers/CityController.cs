using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shipping.Api.Shared;
using Shipping.Application.Features.Cities.Commands.CreateCity;
using Shipping.Application.Features.Cities.Commands.DeleteCity;
using Shipping.Application.Features.Cities.Commands.UpdateCity;
using Shipping.Application.Features.Cities.Queries;
using Shipping.Application.Features.Cities.Queries.GetCities;
using Shipping.Application.Features.Cities.Queries.GetCitiesByBranchId;
using Shipping.Utils.Vm;

namespace Shipping.Api.Controllers;

public class CityController : BaseController
{
    private readonly IMediator _mediator;

    public CityController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("CreateCity")]
    [Authorize(Roles = "Employee , Owner")]
    public async Task<OperationResult<string>> CreateCity(CreateCityRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    }

    [HttpPost("UpdateCity")]
    [Authorize(Roles = "Employee , Owner")]
    public async Task<OperationResult<string>> UpdateCity([FromBody] UpdateCityRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToOperationResult();
    }
    [HttpGet("GetCitiesByBranchId")]  
    [Authorize(Roles = "User , Owner")]
    public async Task<OperationResult<List<CitiesResopnse>>> GetCitiesByBranchId([FromQuery]GetCitiesByBranchIdRequest request,CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    } 
    [HttpGet("GetCities")]
    [Authorize(Roles = "Employee , Owner")]
    public async Task<OperationResult<List<CitiesResopnse>>> GetCities(CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(new GetCitiesRequest()
        {
            UserId = GetUserId()
        }, cancellationToken);

        return result.ToOperationResult();
    }
    [HttpDelete("DeleteCity")]  
    [Authorize(Roles = "Employee , Owner")]
    public async Task<OperationResult<string>> DeleteCity([FromQuery]DeleteCityRequest request,CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    } 
    
    
}