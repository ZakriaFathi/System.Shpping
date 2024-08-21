namespace Shipping.Application.Features.Branchs.Queries;

public class BranchsResopnse
{
    public Guid BranchId { get; set; }
    public string BranchName { get; set; }
    public bool IsMajor { get; set; }
}