using FluentResults;
using MediatR;

namespace Shipping.Application.Features.UserManagement.Roles.Commands.CreateRole;

public class CreateRoleRequest : IRequest<Result<string>>
{
    public string RoleName{ get; set; }
}