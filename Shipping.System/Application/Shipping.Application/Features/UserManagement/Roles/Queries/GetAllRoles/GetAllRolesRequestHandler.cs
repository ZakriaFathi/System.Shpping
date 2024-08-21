using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.UserManagement.Roles.Queries.GetAllRoles;

public class GetAllRolesRequestHandler : IRequestHandler<GetAllRolesRequest, Result<List<GetRolesResponse>>>
{
    private readonly IRoleRepository _roleRepository;

    public GetAllRolesRequestHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<Result<List<GetRolesResponse>>> Handle(GetAllRolesRequest request,
        CancellationToken cancellationToken)
        => await _roleRepository.GetAllRoles(request, cancellationToken);
}