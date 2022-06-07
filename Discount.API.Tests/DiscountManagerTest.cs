using System;
using Discount.API.Dtos;
using Discount.API.Managers;
using Discount.API.Models;
using Moq;
using Xunit;

namespace Discount.API.Tests;

public class DiscountManagerTest : IDisposable
{
    private readonly Mock<IInvoiceManager> _invoiceManager;
    private readonly DiscountManager _discountManager;

    public DiscountManagerTest()
    {
        _invoiceManager = new Mock<IInvoiceManager>();
        _discountManager = new DiscountManager(_invoiceManager.Object);
    }

    public void Dispose()
    {
        _invoiceManager.VerifyAll();
    }

    [Fact]
    public void get_final_amount_case_1_if_the_user_is_an_employee_than_percent_30_test()
    {
        // setup
        var invoiceId = Guid.NewGuid();
        var userDto = new UserDto { IsEmployee = true };
        var invoiceDto = new InvoiceDto { Id = invoiceId, User = userDto, Amount = 1000 };
        _invoiceManager.Setup(s => s.GetInvoice(It.IsAny<Guid>())).Returns(invoiceDto);

        // act
        var requestModel = new RequestModel { InvoiceId = invoiceId };
        var result = _discountManager.GetFinalAmount(requestModel);

        // assert
        Assert.True(result.DiscountAmount == 300);
        Assert.True(result.FinalAmount == 700);
    }

    [Fact]
    public void get_final_amount_case_2_if_the_user_is_an_affiliate_than_percent_10_test()
    {
        // setup
        var invoiceId = Guid.NewGuid();
        var affiliateId = Guid.NewGuid();
        var userDto = new UserDto { IsEmployee = false };
        var invoiceDto = new InvoiceDto { Id = invoiceId, User = userDto, AffiliateId = affiliateId, Amount = 1000 };
        _invoiceManager.Setup(s => s.GetInvoice(It.IsAny<Guid>())).Returns(invoiceDto);

        // act
        var requestModel = new RequestModel { InvoiceId = invoiceId };
        var result = _discountManager.GetFinalAmount(requestModel);

        // assert
        Assert.True(result.DiscountAmount == 100);
        Assert.True(result.FinalAmount == 900);
    }

    [Fact]
    public void get_final_amount_case_3_if_the_user_has_been_a_customer_for_over_2_years_than_percent_5_test()
    {
        // setup
        var invoiceId = Guid.NewGuid();
        var userDto = new UserDto { IsEmployee = false, CreatedDate = DateTime.Today.AddDays(-735) };
        var invoiceDto = new InvoiceDto { Id = invoiceId, User = userDto, Amount = 1000 };
        _invoiceManager.Setup(s => s.GetInvoice(It.IsAny<Guid>())).Returns(invoiceDto);

        // act
        var requestModel = new RequestModel { InvoiceId = invoiceId };
        var result = _discountManager.GetFinalAmount(requestModel);

        // assert
        Assert.True(result.DiscountAmount == 50);
        Assert.True(result.FinalAmount == 950);
    }

    [Fact]
    public void get_final_amount_case_4_for_every_100_usd_on_the_bill_there_would_be_a_5_usd_test()
    {
        // setup

        var invoiceId = Guid.NewGuid();
        var userDto = new UserDto { IsEmployee = false, CreatedDate = DateTime.Today.AddDays(-100) };
        var invoiceDto = new InvoiceDto { Id = invoiceId, User = userDto, Amount = 1011 };
        _invoiceManager.Setup(s => s.GetInvoice(It.IsAny<Guid>())).Returns(invoiceDto);

        // act
        var requestModel = new RequestModel { InvoiceId = invoiceId };
        var result = _discountManager.GetFinalAmount(requestModel);

        // assert
        Assert.True(result.DiscountAmount == 50);
        Assert.True(result.FinalAmount == 961);
    }

    [Fact]
    public void get_final_amount_case_5_the_percentage_based_discounts_do_not_apply_on_groceries_test()
    {
        // setup
        var invoiceId = Guid.NewGuid();
        var userDto = new UserDto { IsEmployee = true };
        var invoiceDto = new InvoiceDto { Id = invoiceId, User = userDto, IsGroceries = true, Amount = 1000 };
        _invoiceManager.Setup(s => s.GetInvoice(It.IsAny<Guid>())).Returns(invoiceDto);

        // act
        var requestModel = new RequestModel { InvoiceId = invoiceId };
        var result = _discountManager.GetFinalAmount(requestModel);

        // assert
        Assert.True(result.DiscountAmount == 0);
        Assert.True(result.FinalAmount == 1000);
    }
}