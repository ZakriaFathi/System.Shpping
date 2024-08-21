using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.UserManagement.Roles.Queries.GetRolesByUserId;

public class GetRolesByUserIdRequestHandler : IRequestHandler<GetRolesByUserIdRequest, Result<List<GetRolesResponse>>>
{
    private readonly IRoleRepository _roleRepository;

    public GetRolesByUserIdRequestHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Result<List<GetRolesResponse>>> Handle(GetRolesByUserIdRequest request,
        CancellationToken cancellationToken)
        => await _roleRepository.GetRolesByUserId(request, cancellationToken);
}