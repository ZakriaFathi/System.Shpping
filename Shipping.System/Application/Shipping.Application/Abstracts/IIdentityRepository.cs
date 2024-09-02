using FluentResults;
using Shipping.Application.Models.IdentityModel;
using Shipping.Domain.Models;

namespace Shipping.Application.Abstracts;

public interface IIdentityRepository
{
    Task<Result<AppUser>> GetIdentityUserById(string userId, CancellationToken cancellationToken);
    Task<Result<AppUser>> GetIdentityUserByUserName(string userName, CancellationToken cancellationToken);
    Task<Result<string>> ResetIdentityPassword(ResetIdentityPassword command, CancellationToken cancellationToken);

    Task<Result<AppUser>> SingUp(SingUpCommnd request, CancellationToken cancellationToken);
    Task<Result<AppUser>> InsertIdentityUser(InsertAndUpdateIdentityUser command, CancellationToken cancellationToken);
    Task<Result<string>> UpdateIdentityCustomer(InsertAndUpdateIdentityUser command, CancellationToken cancellationToken);
    Task<Result<string>> ChangeIdentityActivation(ChangeIdentityActivation command, CancellationToken cancellationToken);
    Task<Result<string>> InsertIdentityUserClaims(InsertAndUpdateIdentityClaims command, CancellationToken cancellationToken);
    Task<Result<string>> UpdateIdentityUserClaims(InsertAndUpdateIdentityClaims command, CancellationToken cancellationToken);
}