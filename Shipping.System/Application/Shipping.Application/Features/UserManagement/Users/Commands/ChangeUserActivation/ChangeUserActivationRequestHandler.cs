using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.UserManagement.Users.Commands.ChangeUserActivation;

public class ChangeUserActivationRequestHandler : IRequestHandler<ChangeUserActivationRequest, Result<string>>
{
    private readonly IUserManagmentRepository _userManagementRepository;
    public ChangeUserActivationRequestHandler( IUserManagmentRepository userManagementRepository)
    {
        _userManagementRepository = userManagementRepository;
    }

    public async Task<Result<string>> Handle(ChangeUserActivationRequest request, CancellationToken cancellationToken)
        => await _userManagementRepository.ChangeUserActivationAsync(request, cancellationToken);
}