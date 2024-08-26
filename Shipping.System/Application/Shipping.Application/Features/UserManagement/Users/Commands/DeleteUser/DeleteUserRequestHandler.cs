using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.UserManagement.Users.Commands.DeleteUser;

public class DeleteUserRequestHandler : IRequestHandler<DeleteUserRequest, Result<string>>
{
    private readonly IUserManagmentRepository _userManagment;

    public DeleteUserRequestHandler(IUserManagmentRepository userManagment)
    {
        _userManagment = userManagment;
    }

    public async Task<Result<string>> Handle(DeleteUserRequest request, CancellationToken cancellationToken)
        => await _userManagment.DeleteUser(request, cancellationToken);
}