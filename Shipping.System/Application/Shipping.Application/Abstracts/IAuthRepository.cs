using FluentResults;
using Shipping.Application.Features.Auth.Commands.SingIn;
using Shipping.Application.Features.Auth.Commands.SingUp;

namespace Shipping.Application.Abstracts;

public interface IAuthRepository
{
    Task<Result<string>> SingUp(SingUpRequest request, CancellationToken cancellationToken);
    Task<Result<SingInResponse>> SingInByUserName(SingInRequest request, CancellationToken cancellationToken);
}