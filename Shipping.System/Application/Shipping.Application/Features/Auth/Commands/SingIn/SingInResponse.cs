namespace Shipping.Application.Features.Auth.Commands.SingIn;

public class SingInResponse
{
    public string? Username { get; set; }
    public string BranchId { get; set; }
    public List<string> Roles { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresOn { get; set; }
}