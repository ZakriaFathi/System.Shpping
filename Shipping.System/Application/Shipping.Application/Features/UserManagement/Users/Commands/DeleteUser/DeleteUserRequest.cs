using FluentResults;
using MediatR;

namespace Shipping.Application.Features.UserManagement.Users.Commands.DeleteUser;

public class DeleteUserRequest : IRequest<Result<string>>
{
    public Guid Id { get; set; }

}