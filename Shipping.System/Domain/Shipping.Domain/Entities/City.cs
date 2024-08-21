namespace Shipping.Domain.Entities;

public class City
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal? Price { get; set; }
    
    public Guid BranchId { get; set; }
    public Branch Branch { get; set; }
}