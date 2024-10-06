using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.UserManagement.Users.Commands.ChangePassword;

public class ChangePasswordRequestHandler : IRequestHandler<ChangePasswordRequest, Result<string>>
{
    private readonly IUserManagmentRepository _userManagementRepository;
    public ChangePasswordRequestHandler(IUserManagmentRepository userManagementRepository)
    {
        _userManagementRepository = userManagementRepository;
    }
    public async Task<Result<string>> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
    => await _userManagementRepository.ChangePasswordAsync(request, cancellationToken);
}