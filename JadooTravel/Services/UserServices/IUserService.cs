using JadooTravel.Dtos.UserDtos;
using JadooTravel.Entities;

namespace JadooTravel.Services.UserServices
{
    public interface IUserService
    {
        Task CreateUserAsync(CreateUserDto createUserDto);  
        Task UpdateUserAsync(UpdateUserDto updateUserDto);
        Task UpdatePasswordAsync(UpdateUserPasswordDto updateUserPasswordDto);
        Task<GetUserByIdDto?> GetUserByIdAsync(string userId);
        Task<ResultUserDto?> LoginUserAsync(LoginUserDto loginUserDto);    
        Task<ResultUserDto?> GetUserAsync(string userId);
    }
}
