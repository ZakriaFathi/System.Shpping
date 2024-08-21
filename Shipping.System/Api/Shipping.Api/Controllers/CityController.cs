using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shipping.Application.Features.Cities.Commands.CreateCity;
using Shipping.Application.Features.Cities.Commands.UpdateCity;
using Shipping.Application.Features.Cities.Queries;
using Shipping.Application.Features.Cities.Queries.GetCities;
using Shipping.Application.Features.Cities.Queries.GetCitiesByBranchId;
using Shipping.Utils.Vm;

namespace Shipping.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CityController : ControllerBase
{
    private readonly IMediator _mediator;

    public CityController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("CreateCity")]
    public async Task<OperationResult<string>> CreateCity([FromBody] CreateCityRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    }

    [HttpPost("UpdateCity")]
    public async Task<OperationResult<string>> UpdateCity([FromBody] UpdateCityRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result.ToOperationResult();
    }
    [HttpGet("GetCities")]  
    public async Task<OperationResult<List<CitiesResopnse>>> GetCities(CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(new GetCitiesRequest(), cancellationToken);

        return result.ToOperationResult();
    } 
    
    [HttpGet("GetCitiesByBranchId")]  
    public async Task<OperationResult<List<CitiesResopnse>>> GetCitiesByBranchId([FromQuery]GetCitiesByBranchIdRequest request,CancellationToken cancellationToken)
    { 
        var result = await _mediator.Send(request, cancellationToken);

        return result.ToOperationResult();
    }
}