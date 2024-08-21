using FluentResults;
using MediatR;

namespace Shipping.Application.Features.UserManagement.Users.Commands.ChangePassword;

public class ChangePasswordRequest : IRequest<Result<string>>
{
    public string UserId { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string ConfirmNewPassWord { get; set; }

}