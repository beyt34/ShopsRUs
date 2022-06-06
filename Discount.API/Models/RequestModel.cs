namespace Discount.API.Models;

public class RequestModel
{
    public Guid UserId { get; set; }

    public decimal Amount { get; set; }
}