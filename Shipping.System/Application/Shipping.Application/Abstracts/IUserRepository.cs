using FluentResults;
using Shipping.Application.Models.UserManagement;

namespace Shipping.Application.Abstracts;

public interface IUserRepository
{
    Task<Result<string>> InsertUserAsync(InsertAndUpdateUserCommnd request, CancellationToken cancellationToken);
    Task<Result<string>> InsertEmployeeAsync(InsertAndUpdateEmployeeCommnd request, CancellationToken cancellationToken);
    Task<Result<string>> InsertRepresentativeAsync(InsertAndUpdateRepresentativeCommnd request, CancellationToken cancellationToken);
    Task<Result<string>> InsertCustomerAsync(InsertAndUpdateCustomerCommnd request, CancellationToken cancellationToken);
    
    Task<Result<string>> ChangeUserActivationAsync(ChangeUserActivationCommnd request, CancellationToken cancellationToken);
    Task<Result<string>> ChangePassword(ChangePasswordCommand request, CancellationToken cancellationToken);

    Task<Result<string>> UpdatePasswordAsync(UpdatePasswordCommnd request, CancellationToken cancellationToken);
    Task<Result<string>> UpdateRepresentativeAsync(InsertAndUpdateRepresentativeCommnd request, CancellationToken cancellationToken);
    Task<Result<string>> UpdateEmployeeAsync(InsertAndUpdateEmployeeCommnd request, CancellationToken cancellationToken);
    Task<Result<string>> UpdateCustomerAsync(InsertAndUpdateCustomerCommnd request, CancellationToken cancellationToken);
    Task<Result<string>> CreateUserPermissions(InsertAndUpdateUserPermissions request, CancellationToken cancellationToken);
    Task<Result<string>> UpdateUserPermissions(InsertAndUpdateUserPermissions request, CancellationToken cancellationToken);
}