
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using TaskMaster.Application.DTOs.Auth;
using TaskMaster.Application.DTOs.Profile;
using TaskMaster.Application.Interfaces;
using TaskMaster.Domain.Entities;

namespace TaskMaster.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> userManager;
        public UserService(
            UserManager<User> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<ArrayList> GetAllUsersAsync()
        {
            var users = userManager.Users;
            ArrayList usersDto = MapUserToCollectionDto(users);
            bool arrgument = usersDto.Count == 0 ? true:false;

            return usersDto;
        }
        public async Task<ProfileResultDto> GetProfileAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty", nameof(email));
            }

            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return null;
            }

            return MapUserToDto(user);
        }

        private ProfileResultDto MapUserToDto(User user)
        {
            return new ProfileResultDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Avatar = user.Avatar,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                LastLogoutAt = user.LastLogoutAt
            };
        }
        private ArrayList MapUserToCollectionDto(IQueryable<User> users)
        {
            ArrayList ListProfileDto=new ArrayList();

            foreach (User user in users)
            {
                ProfileResultDto profileDto = MapUserToDto(user);
                ListProfileDto.Add(profileDto);
            }
            return ListProfileDto;
        }
    }
}
