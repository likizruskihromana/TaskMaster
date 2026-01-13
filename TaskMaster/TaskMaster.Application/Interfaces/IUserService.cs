using System.Collections;
using TaskMaster.Application.DTOs.Auth;
using TaskMaster.Application.DTOs.Profile;

public interface IUserService
{
    Task<ArrayList> GetAllUsersAsync();
    Task<ProfileResultDto> GetProfileAsync(string email);
    Task<UserDto> GetUserByEmailAsync(string email);
    Task<UserDto> GetUserByIdAsync(int id);
    Task<ProfileResultDto> UpdateProfileAsync(int userId, UpdateProfileDto dto);
    Task<Boolean> UpdatePasswordAsync(int userId, UpdatePasswordDto dto);
}