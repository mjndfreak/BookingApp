using BookingApp.Business.DataProtection;
using BookingApp.Business.Operations.User.Dtos;
using BookingApp.Business.Types;
using BookingApp.Data.Entities;
using BookingApp.Data.Enums;
using BookingApp.Data.Repositories;
using BookingApp.Data.UnitOfWork;

namespace BookingApp.Business.Operations.User;

public class UserManager : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<UserEntity> _userRepository;
    private readonly IDataProtection _protector;


    public UserManager(IUnitOfWork unitOfWork, IRepository<UserEntity> userRepository, IDataProtection protector)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _protector = protector ?? throw new ArgumentNullException(nameof(protector));
    }

    public async Task<ServiceMessage> AddUserAsync(AddUserDto user)
    {
        var hasMail = _userRepository.GetAll(x => x.Email == user.Email);
        if (hasMail.Any())
        {
            return new ServiceMessage
            {
                IsSuccess = false,
                Message = "This email is already in use."
            };
        }
        var userEntity = new UserEntity
        {
            Email = user.Email,
            Password = _protector.Protect(user.Password),
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthDate = user.BirthDate.ToUniversalTime(),
            UserType = UserType.Admin
        };
        _userRepository.Add(userEntity);
        try
        {
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw new Exception("An error occurred while saving the entity changes.");
        }

        return new ServiceMessage
        {
            IsSuccess = true
        };
    }

    public ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user)
    {
        var userEntity = _userRepository.Get(x => x.Email.ToLower() == user.Email);
        if (userEntity == null)
        {
            return new ServiceMessage<UserInfoDto>
            {
                IsSuccess = false,
                Message = "User or password is incorrect."
            };
        }
        
        var unprotectedPassword = _protector.Unprotect(userEntity.Password);

        if (unprotectedPassword == user.Password)
        {
            return new ServiceMessage<UserInfoDto>
            {
                IsSuccess = true,
                Data = new UserInfoDto
                {
                    Email = userEntity.Email,
                    FirstName = userEntity.FirstName,
                    LastName = userEntity.LastName,
                    UserType = userEntity.UserType,
                }
            };
        }

        return new ServiceMessage<UserInfoDto>
        {
            IsSuccess = false,
            Message = "Username or password is incorrect."
        };
    }
}