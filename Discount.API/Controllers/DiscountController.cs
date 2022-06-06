using Discount.API.Managers;
using Discount.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Discount.API.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class DiscountController : ControllerBase
{
    private readonly IDiscountManager _discountManager;

    public DiscountController(IDiscountManager discountManager)
    {
        _discountManager = discountManager;
    }

    [HttpPost]
    public ResponseModel GetFinalAmount([FromBody] RequestModel requestModel) => _discountManager.GetFinalAmount(requestModel);
}