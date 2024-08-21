using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.UserManagement.Permissions.Commands.CreatePermission;

public class CreatePermissionRequestHandler : IRequestHandler<CreatePermissionRequest, Result<string>>
{
    private readonly IPermissionsRepository _permissionsRepository;

    public CreatePermissionRequestHandler(IPermissionsRepository permissionsRepository)
    {
        _permissionsRepository = permissionsRepository;
    }

    public async Task<Result<string>> Handle(CreatePermissionRequest request, CancellationToken cancellationToken)
        => await _permissionsRepository.CreatePermission(request, cancellationToken);
}