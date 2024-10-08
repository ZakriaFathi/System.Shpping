using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Domain.Entities;

public class Customer
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    
    public ICollection<Order> Orders { get; set; }

    [ForeignKey("UserId")]
    public Guid UserId { get; set; }
    public User User { get; set; }
}