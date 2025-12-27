using System.Collections;
using TaskMaster.Application.DTOs.Auth;
using TaskMaster.Application.DTOs.Profile;

public interface IUserService
{
    Task<ArrayList> GetAllUsersAsync();
    Task<ProfileResultDto> GetProfileAsync(string email);
}