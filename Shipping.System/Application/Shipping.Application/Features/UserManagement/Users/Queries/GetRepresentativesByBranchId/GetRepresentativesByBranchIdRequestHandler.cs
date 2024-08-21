using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.UserManagement.Users.Queries.GetRepresentativesByBranchId;

public class GetRepresentativesByBranchIdRequestHandler : IRequestHandler<GetRepresentativesByBranchIdRequest, Result<List<GetUsersResponse>>>
{
    private readonly IUserManagmentRepository _userManagementRepository;

    public GetRepresentativesByBranchIdRequestHandler(IUserManagmentRepository userManagementRepository)
    {
        _userManagementRepository = userManagementRepository;
    }

    public async Task<Result<List<GetUsersResponse>>> Handle(GetRepresentativesByBranchIdRequest request, CancellationToken cancellationToken)
        => await _userManagementRepository.GetRepresentativesByBranchIdAsync(request, cancellationToken);
}