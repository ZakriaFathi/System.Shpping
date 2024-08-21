using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.UserManagement.Users.Queries.GetRepresentatives;

public class GetRepresentativesRequestHandler : IRequestHandler<GetRepresentativesRequest, Result<List<GetUsersResponse>>>
{
    private readonly IUserManagmentRepository _userManagementRepository;

    public GetRepresentativesRequestHandler(IUserManagmentRepository userManagementRepository)
    {
        _userManagementRepository = userManagementRepository;
    }

    public async Task<Result<List<GetUsersResponse>>> Handle(GetRepresentativesRequest request, CancellationToken cancellationToken)
        => await _userManagementRepository.GetRepresentativesAsync(request, cancellationToken);
}