using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScienceTrack.Models;
using ScienceTrack.Repositories;
using System.Security.Claims;

namespace ScienceTrack.Services
{
    public class AuthorizationService
    {
        private Repository repository;
        private ILogger logger;
        public AuthorizationService(Repository repository, ILogger<AuthorizationService> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        public async Task<User?> Authorize(HttpContext context, string userName, string password)
        {
            var passwordHasher = new PasswordHasher<User>();
            User? user = repository.Users.GetList().Result
                .FirstOrDefault(u => u.UserName.ToLower() == userName.ToLower() &&
                                     (passwordHasher.VerifyHashedPassword(u, u.PasswordHash, password) == PasswordVerificationResult.Success ||
                                      passwordHasher.VerifyHashedPassword(u, u.PasswordHash, password) == PasswordVerificationResult.SuccessRehashNeeded));
            if (user is null) return null;
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                    new Claim(ClaimTypes.Role, repository.Roles.Get(user.Role.Value).RoleName.ToString())
                };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await context.SignInAsync(claimsPrincipal, new AuthenticationProperties() { IssuedUtc = DateTime.UtcNow, ExpiresUtc = DateTime.UtcNow.AddMinutes(100), AllowRefresh = true });
            logger.LogInformation("user: " + user.UserName + " was authorized");
            return user;
        }
        public async Task<bool> Logout(HttpContext context)
        {
            await context.SignOutAsync();
            return true;
        }

        [Authorize]
        public async Task<User?> IsAuthorize(HttpContext context)
        {
            if (context.User.Claims.FirstOrDefault() == null) return null;
            return repository.Users.context.Users.Include(x => x.RoleNavigation).FirstOrDefault(x => x.UserName == context.User.Claims.First().Value);
        }

        public async Task<User?> Register(string userName, string officialName, string password)
        {
            if (repository.Users.GetList().Result.FirstOrDefault(x => x.UserName.ToLower() == userName.ToLower()) is null)
            {
                var user = new User()
                {
                    UserName = userName,
                    PasswordHash = password,
                    OfficialName = officialName,
                    Role = 2
                };

                user.PasswordHash = new PasswordHasher<User>().HashPassword(user, password);
                repository.Users.Create(user);
                await repository.Users.Save();
                logger.LogInformation("user: " + user.UserName + " was registered");
                return user;
            }
            logger.LogWarning("user is already existing");
            return null;
        }
    }
}
