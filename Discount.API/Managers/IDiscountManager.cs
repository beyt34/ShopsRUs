using Discount.API.Models;

namespace Discount.API.Managers;

public interface IDiscountManager
{
    ResponseModel GetFinalAmount(RequestModel requestModel);
}