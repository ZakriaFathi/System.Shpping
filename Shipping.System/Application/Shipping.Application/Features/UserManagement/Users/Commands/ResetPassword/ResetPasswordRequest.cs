using FluentResults;
using MediatR;

namespace Shipping.Application.Features.UserManagement.Users.Commands.ResetPassword;

public class ResetPasswordRequest : IRequest<Result<string>>
{
    public string UserName { get; set; }

    public string NewPassword { get; set; }
    public string ConfiramNewPassword { get; set; }
}