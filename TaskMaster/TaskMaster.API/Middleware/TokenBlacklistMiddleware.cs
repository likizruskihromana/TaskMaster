using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskMaster.Application.Interfaces;

namespace TaskMaster.API.Middleware
{
    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate next;
        public TokenBlacklistMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task Invoke(HttpContext context, ITokenBlackListService tokenBlackListService)
        {
            //only check if user is authenticated
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var jti = context.User.FindFirstValue(JwtRegisteredClaimNames.Jti);
                if (!string.IsNullOrEmpty(jti))
                {
                    bool isBlacklisted = await tokenBlackListService.IsTokenBlacklistedAsync(jti);
                    if (isBlacklisted)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsJsonAsync(new
                        {
                            message = "Token has been revoked"
                        });
                        return;
                    }
                }
            }
            await next(context);
        }
    }
}
