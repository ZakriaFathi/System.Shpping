using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.UserManagement.Permissions.Queries.GetPermissionsByRoleId;

public class GetAllPermissionsByUserIdRequestHandler : IRequestHandler<GetAllPermissionsByUserIdRequest, Result<List<GetPermissionsResponse>>>
{
    private readonly IPermissionsRepository _permissionsRepository;

    public GetAllPermissionsByUserIdRequestHandler(IPermissionsRepository permissionsRepository)
    {
        _permissionsRepository = permissionsRepository;
    }

    public async Task<Result<List<GetPermissionsResponse>>> Handle(GetAllPermissionsByUserIdRequest request,
        CancellationToken cancellationToken)
        => await _permissionsRepository.GetPermissionsByUserId(request, cancellationToken);
}