using FluentResults;

using Shipping.Application.Models.IdentityModel;
using Shipping.Application.Models.UserManagement;
using Shipping.Domain.Entities;
using Shipping.Domain.Models;

namespace Shipping.Application.Abstracts;

public interface ISherdUserRepository
{
    #region Identity
    Task<Result<AppUser>> GetIdentityUserById(string userId, CancellationToken cancellationToken);
    Task<Result<AppUser>> GetIdentityUserByUserName(string userName, CancellationToken cancellationToken);
    Task<Result<string>> ResetIdentityPassword(ResetIdentityPassword command, CancellationToken cancellationToken);

    Task<Result<AppUser>> SingUp(SingUpCommnd request, CancellationToken cancellationToken);
    Task<Result<AppUser>> InsertIdentityUser(InsertAndUpdateIdentityUser command, CancellationToken cancellationToken);
    Task<Result<string>> UpdateIdentityCustomer(InsertAndUpdateIdentityUser command, CancellationToken cancellationToken);
    Task<Result<string>> ChangeIdentityPassword(ChangeIdentityPassword command, CancellationToken cancellationToken);
    Task<Result<string>> ChangeIdentityActivation(ChangeIdentityActivation command, CancellationToken cancellationToken);
    Task<Result<string>> InsertIdentityUserClaims(InsertAndUpdateIdentityClaims command, CancellationToken cancellationToken);
    Task<Result<string>> UpdateIdentityUserClaims(InsertAndUpdateIdentityClaims command, CancellationToken cancellationToken);
    #endregion
    
    #region UserManagment
    Task<Result<string>> InsertUserAsync(InsertAndUpdateUserCommnd request, CancellationToken cancellationToken);
    Task<Result<string>> InsertEmployeeAsync(InsertAndUpdateEmployeeCommnd request, CancellationToken cancellationToken);
    Task<Result<string>> InsertRepresentativeAsync(InsertAndUpdateRepresentativeCommnd request, CancellationToken cancellationToken);
    Task<Result<string>> InsertCustomerAsync(InsertAndUpdateCustomerCommnd request, CancellationToken cancellationToken);
    
    Task<Result<string>> ChangePasswordUserAsync(ChangePasswordCommand request, CancellationToken cancellationToken);
    Task<Result<string>> ChangeUserActivationAsync(ChangeUserActivationCommnd request, CancellationToken cancellationToken);
    Task<Result<string>> UpdateUserAsync(InsertAndUpdateUserCommnd request, CancellationToken cancellationToken);
    Task<Result<string>> UpdatePasswordAsync(UpdatePasswordCommnd request, CancellationToken cancellationToken);
    Task<Result<string>> UpdateCustomerAsync(InsertAndUpdateCustomerCommnd request, CancellationToken cancellationToken);
    Task<Result<string>> UpdateRepresentativeAsync(InsertAndUpdateRepresentativeCommnd request, CancellationToken cancellationToken);
    Task<Result<string>> UpdateEmployeeAsync(InsertAndUpdateEmployeeCommnd request, CancellationToken cancellationToken);
    Task<Result<string>> CreateUserPermissions(InsertAndUpdateUserPermissions request, CancellationToken cancellationToken);
    Task<Result<string>> UpdateUserPermissions(InsertAndUpdateUserPermissions request, CancellationToken cancellationToken);
    #endregion
}