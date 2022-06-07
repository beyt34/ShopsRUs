using Discount.API.Models;

namespace Discount.API.Managers;

public class DiscountManager : IDiscountManager
{
    private readonly IInvoiceManager _invoiceManager;

    public DiscountManager(IInvoiceManager invoiceManager)
    {
        _invoiceManager = invoiceManager;
    }

    public ResponseModel GetFinalAmount(RequestModel requestModel)
    {
        var responseModel = new ResponseModel { InvoiceId = requestModel.InvoiceId };
        var invoiceDto = _invoiceManager.GetInvoice(requestModel.InvoiceId);

        if (invoiceDto != null)
        {
            // set amount
            responseModel.Amount = invoiceDto.Amount;

            // The percentage based discounts do not apply on groceries.
            if (invoiceDto.IsGroceries)
            {
                SetAmounts(responseModel, 0);
            }
            // If the user is an employee of the store, he gets a 30 % discount
            else if (invoiceDto.User.IsEmployee)
            {
                SetAmounts(responseModel, 30);
            }
            // If the user is an affiliate of the store, he gets a 10% discount
            else if (invoiceDto.AffiliateId != Guid.Empty)
            {
                SetAmounts(responseModel, 10);
            }
            // If the user has been a customer for over 2 years, he gets a 5% discount.
            else if (invoiceDto.User.OldCustomer)
            {
                SetAmounts(responseModel, 5);
            }
            // For every $100 on the bill, there would be a $ 5 discount (e.g. for $ 990, you get $ 45 as a discount).
            else
            {
                SetAmounts(responseModel);
            }
        }

        return responseModel;
    }

    private void SetAmounts(ResponseModel responseModel, decimal percentageDiscount)
    {
        responseModel.DiscountAmount = responseModel.Amount * percentageDiscount / 100.0M;
        responseModel.FinalAmount = responseModel.Amount - responseModel.DiscountAmount;
    }

    private void SetAmounts(ResponseModel responseModel)
    {
        responseModel.DiscountAmount = Math.Floor(responseModel.Amount / 100) * 5;
        responseModel.FinalAmount = responseModel.Amount - responseModel.DiscountAmount;
    }
}