using FluentResults;
using MediatR;
using Shipping.Utils.Enums;

namespace Shipping.Application.Features.UserManagement.Users.Commands.ChangeUserActivation;

public class ChangeUserActivationRequest : IRequest<Result<string>>
{
    public string UserId { get; set; }
    public ActivateState State { get; set; }
}