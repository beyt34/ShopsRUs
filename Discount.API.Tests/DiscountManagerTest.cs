using System;
using Discount.API.Dtos;
using Discount.API.Managers;
using Discount.API.Models;
using Moq;
using Xunit;

namespace Discount.API.Tests;

public class DiscountManagerTest : IDisposable
{
    private readonly Mock<IUserManager> _userManager;
    private readonly DiscountManager _discountManager;

    public DiscountManagerTest()
    {
        _userManager = new Mock<IUserManager>();
        _discountManager = new DiscountManager(_userManager.Object);
    }

    public void Dispose()
    {
        _userManager.VerifyAll();
    }

    [Fact]
    public void get_final_amount_case_1_if_the_user_is_an_employee_than_percent_30_test()
    {
        // setup
        var userDto = new UserDto { IsEmployee = true };
        _userManager.Setup(s => s.GetUser(It.IsAny<Guid>())).Returns(userDto);

        // act
        var requestModel = new RequestModel { UserId = Guid.NewGuid(), Amount = 1000 };
        var result = _discountManager.GetFinalAmount(requestModel);

        // assert
        Assert.True(result.DiscountAmount == 300);
        Assert.True(result.FinalAmount == 700);
    }

    [Fact]
    public void get_final_amount_case_2_if_the_user_is_an_affiliate_than_percent_10_test()
    {
        // setup
        var userDto = new UserDto { IsEmployee = false, IsAffiliate = true };
        _userManager.Setup(s => s.GetUser(It.IsAny<Guid>())).Returns(userDto);

        // act
        var requestModel = new RequestModel { UserId = Guid.NewGuid(), Amount = 1000 };
        var result = _discountManager.GetFinalAmount(requestModel);

        // assert
        Assert.True(result.DiscountAmount == 100);
        Assert.True(result.FinalAmount == 900);
    }

    [Fact]
    public void get_final_amount_case_3_if_the_user_has_been_a_customer_for_over_2_years_than_percent_5_test()
    {
        // setup
        var userDto = new UserDto { IsEmployee = false, IsAffiliate = false, CreatedDate = DateTime.Today.AddDays(-735) };
        _userManager.Setup(s => s.GetUser(It.IsAny<Guid>())).Returns(userDto);

        // act
        var requestModel = new RequestModel { UserId = Guid.NewGuid(), Amount = 1000 };
        var result = _discountManager.GetFinalAmount(requestModel);

        // assert
        Assert.True(result.DiscountAmount == 50);
        Assert.True(result.FinalAmount == 950);
    }

    [Fact]
    public void get_final_amount_case_4_for_every_100_usd_on_the_bill_there_would_be_a_5_usd_test()
    {
        // setup
        var userDto = new UserDto { IsEmployee = false, IsAffiliate = false, CreatedDate = DateTime.Today.AddDays(-100) };
        _userManager.Setup(s => s.GetUser(It.IsAny<Guid>())).Returns(userDto);

        // act
        var requestModel = new RequestModel { UserId = Guid.NewGuid(), Amount = 1011 };
        var result = _discountManager.GetFinalAmount(requestModel);

        // assert
        Assert.True(result.DiscountAmount == 50);
        Assert.True(result.FinalAmount == 961);
    }
}