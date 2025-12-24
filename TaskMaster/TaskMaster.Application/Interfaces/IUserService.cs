using TaskMaster.Application.DTOs.Auth;
using TaskMaster.Application.DTOs.Profile;

public interface IUserService
{
    Task<ProfileResultDto> GetAllUsersAsync(RegisterDto registerDto);
}