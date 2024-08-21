using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.UserManagement.Users.Commands.UpdateUser;

public class UpdateUserRequestHandler : IRequestHandler<UpdateUserRequest, Result<string>>
{
    private readonly IUserManagmentRepository _userManagementRepository;

    public UpdateUserRequestHandler(IUserManagmentRepository userManagementRepository)
    {
        _userManagementRepository = userManagementRepository;
    }
    
    public async Task<Result<string>> Handle(UpdateUserRequest request, CancellationToken cancellationToken)
        => await _userManagementRepository.UpdateUserAsync(request, cancellationToken);
}