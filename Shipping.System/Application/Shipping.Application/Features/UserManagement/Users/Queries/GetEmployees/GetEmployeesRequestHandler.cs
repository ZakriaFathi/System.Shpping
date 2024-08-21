using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.UserManagement.Users.Queries.GetAdmins;

public class GetEmployeesRequestHandler : IRequestHandler<GetEmployeesRequest, Result<List<GetUsersResponse>>>
{
    private readonly IUserManagmentRepository _userManagementRepository;

    public GetEmployeesRequestHandler(IUserManagmentRepository userManagementRepository)
    {
        _userManagementRepository = userManagementRepository;
    }

    public async Task<Result<List<GetUsersResponse>>> Handle(GetEmployeesRequest request, CancellationToken cancellationToken)
        => await _userManagementRepository.GetEmployeesAsync(request, cancellationToken);
}