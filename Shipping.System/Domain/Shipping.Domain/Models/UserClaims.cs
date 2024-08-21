namespace Shipping.Domain.Models;

public class UserClaims
{
    public string type { get; set; }
    public List<string> value { get; set; } = new List<string>();
}