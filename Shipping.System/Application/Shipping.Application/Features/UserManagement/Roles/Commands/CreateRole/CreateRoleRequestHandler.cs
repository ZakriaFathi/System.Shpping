using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.UserManagement.Roles.Commands.CreateRole;

public class CreateRoleRequestHandler : IRequestHandler<CreateRoleRequest, Result<string>>
{
    private readonly IRoleRepository _roleRepository;

    public CreateRoleRequestHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Result<string>> Handle(CreateRoleRequest request, CancellationToken cancellationToken)
        => await _roleRepository.CreateRole(request, cancellationToken);
}