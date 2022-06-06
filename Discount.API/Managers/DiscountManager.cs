using Discount.API.Models;

namespace Discount.API.Managers;

public class DiscountManager : IDiscountManager
{
    private readonly IUserManager _userManager;

    public DiscountManager(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public ResponseModel GetFinalAmount(RequestModel requestModel)
    {
        var responseModel = new ResponseModel { UserId = requestModel.UserId, Amount = requestModel.Amount };
        var userDto = _userManager.GetUser(requestModel.UserId);

        if (userDto != null)
        {
            // 1.If the user is an employee of the store, he gets a 30 % discount
            if (userDto.IsEmployee)
            {
                responseModel.DiscountAmount = responseModel.Amount * 0.3M;
                responseModel.FinalAmount = responseModel.Amount - responseModel.DiscountAmount;
            }
            // 2.If the user is an affiliate of the store, he gets a 10% discount
            else if (userDto.IsAffiliate)
            {
                responseModel.DiscountAmount = responseModel.Amount * 0.1M;
                responseModel.FinalAmount = responseModel.Amount - responseModel.DiscountAmount;
            }
            // 3.If the user has been a customer for over 2 years, he gets a 5% discount.
            else if (userDto.OldCustomer)
            {
                responseModel.DiscountAmount = responseModel.Amount * 0.05M;
                responseModel.FinalAmount = responseModel.Amount - responseModel.DiscountAmount;
            }
            // 4.For every $100 on the bill, there would be a $ 5 discount (e.g. for $ 990, you get $ 45 as a discount).
            else if (requestModel.Amount >= 100)
            {
                responseModel.DiscountAmount = responseModel.Amount % 100 * 5;
                responseModel.FinalAmount = responseModel.Amount - responseModel.DiscountAmount;
            }
        }

        return responseModel;
    }
}