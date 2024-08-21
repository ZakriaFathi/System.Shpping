using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.UserManagement.Users.Commands.CreateUser;

public class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, Result<string>>
{
    private readonly IUserManagmentRepository _userManagementRepository;

    public CreateUserRequestHandler(IUserManagmentRepository userManagementRepository)
    {
        _userManagementRepository = userManagementRepository;
    }
    
    public async Task<Result<string>> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        => await _userManagementRepository.CreateUserAsync(request, cancellationToken);
}