using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.UserManagement.Users.Queries.GetAdminsByBranchId;

public class GetEmployeesByBranchIdRequestHandler : IRequestHandler<GetEmployeesByBranchIdRequest, Result<List<GetUsersResponse>>>
{
    private readonly IUserManagmentRepository _userManagementRepository;

    public GetEmployeesByBranchIdRequestHandler(IUserManagmentRepository userManagementRepository)
    {
        _userManagementRepository = userManagementRepository;
    }

    public async Task<Result<List<GetUsersResponse>>> Handle(GetEmployeesByBranchIdRequest request, CancellationToken cancellationToken)
        => await _userManagementRepository.GetEmployeesByBranchIdAsync(request, cancellationToken);
}