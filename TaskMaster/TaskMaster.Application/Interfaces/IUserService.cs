using TaskMaster.Application.DTOs.Auth;
using TaskMaster.Application.DTOs.Profile;

public interface IUserService
{
    Task<ProfileResultDto> ProfileData(RegisterDto registerDto);
    Task<ProfileResultDto> ProfileProjects(LoginDto loginDto);
}