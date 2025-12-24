using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskMaster.Application.DTOs.Auth;
using TaskMaster.Application.Interfaces;
using TaskMaster.Domain.Entities;

namespace TaskMaster.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IConfiguration configuration;
        private readonly ITokenBlackListService tokenBlackListService;
        public AuthService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration,
            ITokenBlackListService tokenBlackListService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.tokenBlackListService = tokenBlackListService;
        }
        public async Task<AuthResultDto> RegisterAsync(RegisterDto dto)
        {
            // 1. Check if user already exists
            var existingUser = await userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                return new AuthResultDto
                {
                    Success = false,
                    Errors = new[] { "Email is already registered" }
                };
            }

            // 2. Create new user entity
            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow
            };

            // 3. Create user with password (Identity handles hashing)
            var result = await userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return new AuthResultDto
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description)
                };
            }

            // 4. Generate JWT token
            string token = GenerateJwtToken(user);

            // 5. Map User entity to UserDto
            var userDto = MapUserToDto(user);

            // 6. Return success result
            return new AuthResultDto
            {
                Success = true,
                Token = token,
                TokenExpiration = DateTime.UtcNow.AddHours(1),
                User = userDto
            };
        }
        public async Task<AuthResultDto> LoginAsync(LoginDto dto)
        {
            // 1. Find user by email
            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return new AuthResultDto
                {
                    Success = false,
                    Errors = new[] { "Invalid email or password" }
                };
            }

            // 2. Check password and handle lockout
            var result = await signInManager.CheckPasswordSignInAsync(
                user,
                dto.Password,
                lockoutOnFailure: true
            );

            if (result.IsLockedOut)
            {
                return new AuthResultDto
                {
                    Success = false,
                    Errors = new[] { "Account locked. Please try again in 15 minutes." }
                };
            }

            if (!result.Succeeded)
            {
                return new AuthResultDto
                {
                    Success = false,
                    Errors = new[] { "Invalid email or password" }
                };
            }

            // 3. Update last login time
            user.LastLoginAt = DateTime.UtcNow;
            await userManager.UpdateAsync(user);

            // 4. Generate JWT token
            var token = GenerateJwtToken(user);

            // 5. Map User entity to UserDto
            var userDto = MapUserToDto(user);
            Console.WriteLine(userDto);
            // 6. Return success result
            return new AuthResultDto
            {
                Success = true,
                Token = token,
                TokenExpiration = DateTime.UtcNow.AddHours(1),
                User = userDto
            };
        }
        public async Task<bool> LogoutAsync(int userId, string tokenId, DateTime expirationDate)
        {
            // Find user
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return false;
            }

            // Update last logout time
            user.LastLogoutAt = DateTime.UtcNow;

            var result = await userManager.UpdateAsync(user);
            await tokenBlackListService.BlacklistTokenAsync(tokenId, expirationDate);
            return result.Succeeded;
        }
        private string GenerateJwtToken(User user)
        {
            // 1. Create claims (user information stored in token)
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // 2. Get secret key from configuration
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"])
            );

            // 3. Create signing credentials
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 4. Create token descriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = configuration["JwtSettings:Issuer"],
                Audience = configuration["JwtSettings:Audience"],
                SigningCredentials = credentials
            };

            // 5. Generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 6. Return token as string
            return tokenHandler.WriteToken(token);
        }
        private UserDto MapUserToDto(User user)
        {
            return new UserDto
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Avatar = user.Avatar
            };
        }
    }
}