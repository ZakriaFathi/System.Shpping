using FluentResults;
using MediatR;

namespace Shipping.Application.Features.UserManagement.Roles.Commands.DeleteRole;

public class DeleteRoleRequest : IRequest<Result<string>>
{
    public Guid Id { get; set; }

}