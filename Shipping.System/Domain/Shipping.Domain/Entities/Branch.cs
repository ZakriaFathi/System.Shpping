namespace Shipping.Domain.Entities;

public class Branch
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool IsMajor { get; set; }
    
    public ICollection<City> Citys { get; set; }
    public ICollection<Employee> Employees { get; set; }
    public ICollection<Representative> Representatives { get; set; }
}