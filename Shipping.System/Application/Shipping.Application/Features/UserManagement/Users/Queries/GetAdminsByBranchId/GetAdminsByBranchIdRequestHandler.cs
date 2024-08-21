using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.UserManagement.Users.Queries.GetAdminsByBranchId;

public class GetAdminsByBranchIdRequestHandler : IRequestHandler<GetAdminsByBranchIdRequest, Result<List<GetUsersResponse>>>
{
    private readonly IUserManagmentRepository _userManagementRepository;

    public GetAdminsByBranchIdRequestHandler(IUserManagmentRepository userManagementRepository)
    {
        _userManagementRepository = userManagementRepository;
    }

    public async Task<Result<List<GetUsersResponse>>> Handle(GetAdminsByBranchIdRequest request, CancellationToken cancellationToken)
        => await _userManagementRepository.GetAdministratorsByBranchIdAsync(request, cancellationToken);
}