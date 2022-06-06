using Discount.API.Dtos;

namespace Discount.API.Managers;

public class UserManager : IUserManager
{
    public UserDto GetUser(Guid userId)
    {
        var user = new UserDto
        {
            Id = userId,
            IsEmployee = true,
            IsAffiliate = true,
            CreatedDate = DateTime.Today.AddYears(-3),
        };

        return user;
    }
}