using Shipping.Domain.Models;

namespace Shipping.Application.Models.IdentityModel;

public class InsertAndUpdateIdentityClaims
{
    public string UserId { get; set; }
    public List<UserClaims> Claims { get; set; }
}