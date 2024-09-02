using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;
using Shipping.Application.Features.Auth.Commands.UpdateCustomer;
using Shipping.Application.Features.UserManagement.Users.Commands.CreateUser;

namespace Shipping.Application.Features.UserManagement.Users.Commands.UpdateCustomer;

public class UpdateCustomerRequestHandler: IRequestHandler<UpdateCustomerRequest, Result<string>>
{
    private readonly IUserManagmentRepository _userManagementRepository;

    public UpdateCustomerRequestHandler(IUserManagmentRepository userManagementRepository)
    {
        _userManagementRepository = userManagementRepository;
    }
    
    public async Task<Result<string>> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
        => await _userManagementRepository.UpdateCustomerAsync(request, cancellationToken);
}