using FluentResults;
using MediatR;

namespace Shipping.Application.Features.UserManagement.Users.Commands.ChangePassword;

public class ChangePasswordRequest : IRequest<Result<string>>
{
    public string UserId { get; set; }
    public string OldPassWord { get; set; }
    public string NewPassWord { get; set; }
    public string ConfirmNewPassWord { get; set; }
}