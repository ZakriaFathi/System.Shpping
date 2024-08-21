using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.UserManagement.Permissions.Queries.GetAllPermissions;

public class GetAllPermissionsRequestHandler : IRequestHandler<GetAllPermissionsRequest, Result<List<GetPermissionsResponse>>>
{
    private readonly IPermissionsRepository _permissionsRepository;

    public GetAllPermissionsRequestHandler(IPermissionsRepository permissionsRepository)
    {
        _permissionsRepository = permissionsRepository;
    }

    public async Task<Result<List<GetPermissionsResponse>>> Handle(GetAllPermissionsRequest request,
        CancellationToken cancellationToken)
        => await _permissionsRepository.GetAllPermissions(request, cancellationToken);
}