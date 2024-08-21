using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Auth.Commands.SingIn;

public class SingInRequest : IRequest<Result<SingInResponse>>
{
    public string UserName { get; set; }

    public string Password { get; set; }
}