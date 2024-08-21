using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.UserManagement.Users.Commands.UpdateUserPermissions;

public class UpdateUserPermissionsRequestHandler : IRequestHandler<UpdateUserPermissionsRequest, Result<string>>
{
    private readonly IUserManagmentRepository _managmentRepository;

    public UpdateUserPermissionsRequestHandler(IUserManagmentRepository managmentRepository)
    {
        _managmentRepository = managmentRepository;
    }

    public async Task<Result<string>> Handle(UpdateUserPermissionsRequest request, CancellationToken cancellationToken)
        => await _managmentRepository.UpdateUserPermissionsAsync(request, cancellationToken);
}