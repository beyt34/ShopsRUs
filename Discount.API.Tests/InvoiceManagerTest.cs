using System;
using Discount.API.Managers;
using Xunit;

namespace Discount.API.Tests;

public class InvoiceManagerTest : IDisposable
{
    private readonly InvoiceManager _invoiceManager;

    public InvoiceManagerTest()
    {
        _invoiceManager = new InvoiceManager();
    }

    [Fact]
    public void get_invoice_test()
    {
        // setup
        var invoiceId = Guid.NewGuid();

        // act
        var result = _invoiceManager.GetInvoice(invoiceId);

        // assert
        Assert.True(result != null);
        Assert.True(result?.Id == invoiceId);
    }

    public void Dispose()
    {
    }
}