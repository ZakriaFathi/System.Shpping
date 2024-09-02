using Shipping.Utils.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.Domain.Entities;


public class Order
{
    public Guid Id { get; set; }
    public string OrderNo { get; set; }
    public OrderState OrderState { get; set; }
    public string Dscription { get; set; }
    public string RecipientAddress { get; set; }
    public int? CountOfItems { get; set; }
    public string SenderPhoneNo { get; set; }
    public string RecipientPhoneNo { get; set; }
    public decimal? Price { get; set; }
    public decimal? OrderPrice { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Guid CustomerId { get; set; }
    public Customer Customers { get; set; }

    public Guid? RepresentativesId { get; set; }
    public Representative Representatives { get; set; }


    public Guid BranchId { get; set; }
    public Branch Branchs { get; set; }
}