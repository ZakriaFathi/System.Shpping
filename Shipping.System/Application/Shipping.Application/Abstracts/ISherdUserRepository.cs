using FluentResults;
using Shipping.Application.Models.IdentityModel;
using Shipping.Application.Models.UserManagement;
using Shipping.Domain.Entities;
using Shipping.Domain.Models;

namespace Shipping.Application.Abstracts;

public interface ISherdUserRepository
{
    #region Identity
    Task<Result<User>> GetIdentityUserById(string userId, CancellationToken cancellationToken);
    Task<Result<User>> GetIdentityUserByUserName(string userName, CancellationToken cancellationToken);
    Task<Result<User>> SingUp(SingUpCommnd request, CancellationToken cancellationToken);
    Task<Result<User>> InsertIdentityUser(InsertAndUpdateIdentityUser command, CancellationToken cancellationToken);
    Task<Result<string>> UpdateIdentityCustomer(InsertAndUpdateIdentityUser command, CancellationToken cancellationToken);
    Task<Result<string>> ChangeIdentityPassword(ChangeIdentityPassword command, CancellationToken cancellationToken);
    Task<Result<string>> ChangeIdentityActivation(ChangeIdentityActivation command, CancellationToken cancellationToken);
    Task<Result<string>> InsertIdentityUserClaims(InsertAndUpdateIdentityClaims command, CancellationToken cancellationToken);
    Task<Result<string>> UpdateIdentityUserClaims(InsertAndUpdateIdentityClaims command, CancellationToken cancellationToken);
    #endregion
    
    #region UserManagment
    Task<Result<Customer>> GetUserById(Guid userId, CancellationToken cancellationToken);
    Task<Result<string>> InsertUserAsync(InsertAndUpdateUserCommnd request, CancellationToken cancellationToken);
    Task<Result<string>> InsertCustomerAsync(InsertAndUpdateUserCommnd request, CancellationToken cancellationToken);
    Task<Result<string>> ChangePasswordCustomerAsync(ChangePasswordCommand request, CancellationToken cancellationToken);
    Task<Result<string>> ChangeCustomerActivationAsync(ChangeUserActivationCommnd request, CancellationToken cancellationToken);
    Task<Result<string>> UpdateCustomerAsync(InsertAndUpdateUserCommnd request, CancellationToken cancellationToken);
    Task<Result<string>> CreateUserPermissions(InsertAndUpdateUserPermissions request, CancellationToken cancellationToken);
    Task<Result<string>> UpdateUserPermissions(InsertAndUpdateUserPermissions request, CancellationToken cancellationToken);
    #endregion
}