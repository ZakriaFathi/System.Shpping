namespace Shipping.Domain.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public ICollection<Permission> Permissions { get; set; }
    
    
    public static Role Create( Guid id ,string name )
    {
        return new Role()
        {
            Id = id,
            Name = name
        };
    }
}