using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.UserManagement.Permissions.Queries.GetPermissionsByRoleId;

public class GetAllPermissionsByRoleIdRequestHandler : IRequestHandler<GetAllPermissionsByRoleIdRequest, Result<List<GetPermissionsResponse>>>
{
    private readonly IPermissionsRepository _permissionsRepository;

    public GetAllPermissionsByRoleIdRequestHandler(IPermissionsRepository permissionsRepository)
    {
        _permissionsRepository = permissionsRepository;
    }

    public async Task<Result<List<GetPermissionsResponse>>> Handle(GetAllPermissionsByRoleIdRequest request,
        CancellationToken cancellationToken)
        => await _permissionsRepository.GetAllPermissionsByRoleId(request, cancellationToken);
}