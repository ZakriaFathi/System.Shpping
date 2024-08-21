using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.UserManagement.Users.Queries.GetAdmins;

public class GetAdminsRequestHandler : IRequestHandler<GetAdminsRequest, Result<List<GetUsersResponse>>>
{
    private readonly IUserManagmentRepository _userManagementRepository;

    public GetAdminsRequestHandler(IUserManagmentRepository userManagementRepository)
    {
        _userManagementRepository = userManagementRepository;
    }

    public async Task<Result<List<GetUsersResponse>>> Handle(GetAdminsRequest request, CancellationToken cancellationToken)
        => await _userManagementRepository.GetAdministratorsAsync(request, cancellationToken);
}