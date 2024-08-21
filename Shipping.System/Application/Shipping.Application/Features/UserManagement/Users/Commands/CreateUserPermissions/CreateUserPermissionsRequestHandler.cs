using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.UserManagement.Users.Commands.CreateUserPermissions;

public class CreateUserPermissionsRequestHandler : IRequestHandler<CreateUserPermissionsRequest, Result<string>>
{
    private readonly IUserManagmentRepository _managmentRepository;

    public CreateUserPermissionsRequestHandler(IUserManagmentRepository managmentRepository)
    {
        _managmentRepository = managmentRepository;
    }

    public async Task<Result<string>> Handle(CreateUserPermissionsRequest request, CancellationToken cancellationToken)
        => await _managmentRepository.CreateUserPermissionsAsync(request, cancellationToken);
}