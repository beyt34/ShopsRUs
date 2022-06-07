using Discount.API.Dtos;

namespace Discount.API.Managers;

public interface IInvoiceManager
{
    InvoiceDto GetInvoice(Guid invoiceId);
}