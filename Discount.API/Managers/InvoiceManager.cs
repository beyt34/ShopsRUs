using AutoFixture;
using Discount.API.Dtos;

namespace Discount.API.Managers;

public class InvoiceManager : IInvoiceManager
{
    private readonly Fixture _fixture;

    public InvoiceManager()
    {
        _fixture = new Fixture();
    }

    public InvoiceDto GetInvoice(Guid invoiceId)
    {
        var userDto = _fixture.Create<UserDto>();
        var invoiceDto = _fixture.Create<InvoiceDto>();
        invoiceDto.Id = invoiceId;
        invoiceDto.User = userDto;
        return invoiceDto;
    }
}