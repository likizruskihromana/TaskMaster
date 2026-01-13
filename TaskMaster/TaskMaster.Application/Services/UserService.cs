
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null) 
            {
                throw new Exception("Ne postoji korisnik");
            }
            var dto = new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Avatar = user.Avatar
            };
            return dto;
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new Exception("Ne postoji korisnik!");

            }
            var dto = new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Avatar = user.Avatar
            };
            return dto;
        }

        public async Task<ProfileResultDto> UpdateProfileAsync(int userId, UpdateProfileDto dto)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if(user == null)
            {
                throw new Exception("Korisnik ne postoji!");
            }
            if (!string.IsNullOrWhiteSpace(dto.FirstName))
            {
                user.FirstName = dto.FirstName;
            }
            if (!string.IsNullOrWhiteSpace(dto.LastName))
            {
                user.LastName = dto.LastName;
            }
            if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email!=user.Email)
            {
                var emailExist=await userManager.FindByEmailAsync(dto.Email);
                if(emailExist != null)
                {
                    throw new Exception("Već postoji korisnik koji koristi ovaj email!");
                }
                var setEmail = await userManager.SetEmailAsync(user, dto.Email);
                if (!setEmail.Succeeded)
                {
                    throw new Exception("Greška! Nije moguće promjeniti email.");
                }
            }
            if (dto.Avatar != null)
            {
                user.Avatar = dto.Avatar;
            }
            var updateUser = await userManager.UpdateAsync(user);
            if (updateUser == null)
            {
                throw new Exception("Greška podaci nisu promjenili");
            }
            var newProfileData = new ProfileResultDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Avatar = user.Avatar,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                LastLogoutAt = user.LastLogoutAt
            };
            return newProfileData;
        }

        public async Task<Boolean> UpdatePasswordAsync(int userId, UpdatePasswordDto dto)
        {
            if(dto.NewPassword != dto.ConfirmNewPassword)
            {
                throw new Exception("Nisu iste nove šifre.");
            }

            var user = await userManager.FindByIdAsync(userId.ToString());
            if(user == null)
            {
                throw new Exception("Korisnik ne postoji!");
            }
            var changePasswordResult = await userManager.ChangePasswordAsync(user, dto.CurrentPassword,dto.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                throw new Exception("Greška! Šifra nije promjenjena.");
            }
            return true;
        }
    }
}
