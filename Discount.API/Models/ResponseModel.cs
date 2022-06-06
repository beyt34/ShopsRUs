namespace Discount.API.Models;

public class ResponseModel : RequestModel
{
    public decimal DiscountAmount { get; set; }

    public decimal FinalAmount { get; set; }
}