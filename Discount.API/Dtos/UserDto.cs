namespace Discount.API.Dtos;

public class UserDto
{
    public bool IsEmployee { get; set; }

    public bool OldCustomer
    {
        get
        {
            var diff = DateTime.Today.AddYears(-2);
            return diff >= CreatedDate;
        }
    }

    public DateTime CreatedDate { get; set; }
}