using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.Auth.Commands.SingIn;

public class SingInRequestHandler : IRequestHandler<SingInRequest, Result<SingInResponse>>
{
    private readonly IAuthRepository _authService;

    public SingInRequestHandler(IAuthRepository authService)
    {
        _authService = authService;
    }

    public async Task<Result<SingInResponse>> Handle(SingInRequest request, CancellationToken cancellationToken)
        => await _authService.SingInByUserName(request, cancellationToken);
}