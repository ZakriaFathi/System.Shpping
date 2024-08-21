namespace Shipping.Application.Models.UserManagement;

public class InsertAndUpdateCustomerCommnd
{
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
}