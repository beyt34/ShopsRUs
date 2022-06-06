using Discount.API.Dtos;

namespace Discount.API.Managers;

public interface IUserManager
{
    UserDto GetUser(Guid userId);
}