using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.UserManagement.Permissions.Commands.DeletePermission;

public class DeletePermissionRequestHandler : IRequestHandler<DeletePermissionRequest, Result<string>>
{
    private readonly IPermissionsRepository _permissionsRepository;

    public DeletePermissionRequestHandler(IPermissionsRepository permissionsRepository)
    {
        _permissionsRepository = permissionsRepository;
    }

    public async Task<Result<string>> Handle(DeletePermissionRequest request, CancellationToken cancellationToken)
        => await _permissionsRepository.DeleteUserPermissions(request, cancellationToken);
}