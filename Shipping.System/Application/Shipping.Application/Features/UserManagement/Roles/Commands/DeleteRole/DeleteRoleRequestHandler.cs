using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.UserManagement.Roles.Commands.DeleteRole;

public class DeleteRoleRequestHandler : IRequestHandler<DeleteRoleRequest, Result<string>>
{
    private readonly IRoleRepository _roleRepository;

    public DeleteRoleRequestHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Result<string>> Handle(DeleteRoleRequest request, CancellationToken cancellationToken)
        => await _roleRepository.DeleteRole(request, cancellationToken);
}