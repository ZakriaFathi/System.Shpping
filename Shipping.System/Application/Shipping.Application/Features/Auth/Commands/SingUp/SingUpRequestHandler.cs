using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.Auth.Commands.SingUp;

public class SingUpRequestHandler : IRequestHandler<SingUpRequest, Result<string>>
{
    private readonly IAuthRepository _authService;

    public SingUpRequestHandler(IAuthRepository authService)
    {
        _authService = authService;
    }

    public async Task<Result<string>> Handle(SingUpRequest request, CancellationToken cancellationToken)
        => await _authService.SingUp(request, cancellationToken);

}