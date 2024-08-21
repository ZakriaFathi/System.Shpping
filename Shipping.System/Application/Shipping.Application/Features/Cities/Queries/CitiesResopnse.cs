namespace Shipping.Application.Features.Cities.Queries;

public class CitiesResopnse
{
    public Guid BranchId { get; set; }
    public List<Cities> Cities { get; set; }
}

public class Cities
{
    public Guid CityId { get; set; }
    public string Name { get; set; }
    public decimal? Price { get; set; }
}