using BookingApp.Business.Operations.User.Dtos;
using BookingApp.Business.Types;

namespace BookingApp.Business.Operations.User;

public interface IUserService
{
    Task<ServiceMessage> AddUserAsync(AddUserDto user);
    
    ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user);
}