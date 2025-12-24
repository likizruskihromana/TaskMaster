
using TaskMaster.Application.DTOs.Auth;
using TaskMaster.Application.DTOs.Profile;

namespace TaskMaster.Application.Services
{
    public class UserService : IUserService
    {
        public Task<ProfileResultDto> GetAllUsersAsync(RegisterDto registerDto)
        {
            throw new NotImplementedException();
        }
    }
}
