namespace Discount.API.Dtos;

public class InvoiceDto
{
    public Guid Id { get; set; }

    public UserDto User { get; set; }

    public Guid AffiliateId { get; set; }

    public bool IsGroceries { get; set; }

    public decimal Amount { get; set; }
}