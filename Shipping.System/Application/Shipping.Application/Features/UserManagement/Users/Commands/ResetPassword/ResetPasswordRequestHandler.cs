using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.UserManagement.Users.Commands.ResetPassword;

public class ResetPasswordRequestHandler : IRequestHandler<ResetPasswordRequest, Result<string>>
{
    private readonly IUserManagmentRepository _userManagementRepository;

    public ResetPasswordRequestHandler(IUserManagmentRepository userManagementRepository)
    {
        _userManagementRepository = userManagementRepository;
    }

    public async Task<Result<string>> Handle(ResetPasswordRequest request, CancellationToken cancellationToken)
        => await _userManagementRepository.ResetPasswordAsync(request, cancellationToken);
}