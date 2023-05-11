using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ScienceTrack.Repositories;
using System.Security.Claims;

namespace ScienceTrack.Services
{
    public class AuthorizationService
    {
        private Repository repository;
        public AuthorizationService(Repository repository)
        {
            this.repository = repository;
        }

        public async Task<User?> Authorize(HttpContext context, string userName, string password)
        {
            var passwordHasher = new PasswordHasher<User>();
            User? user = repository.Users.GetList().Result.Where(u => u.UserName.ToLower() == userName.ToLower() &&
            (passwordHasher.VerifyHashedPassword(u, u.PasswordHash, password) == PasswordVerificationResult.Success ||
            passwordHasher.VerifyHashedPassword(u, u.PasswordHash, password) == PasswordVerificationResult.SuccessRehashNeeded))
            .FirstOrDefault();
            if (user is null) return null;
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserName)
                };

            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await context.SignInAsync(claimsPrincipal, new AuthenticationProperties() { IssuedUtc = DateTime.UtcNow, ExpiresUtc = DateTime.UtcNow.AddMinutes(100), AllowRefresh = true });
            return user;
        }

        public async Task<bool> Logout(HttpContext context)
        {
            await context.SignOutAsync();
            return true;
        }

        public async Task<User?> Register(HttpContext context, string userName, string password)
        {
            if (repository.Users.GetList().Result.FirstOrDefault(x => x.UserName.ToLower() == userName.ToLower()) is null)
            {
                var user = new User()
                {
                    UserName = userName,
                    PasswordHash = password
                };

                user.PasswordHash = new PasswordHasher<User>().HashPassword(user, password);
                repository.Users.Create(user);
                await repository.Users.Save();

                return await Authorize(context, userName, password);
            }
            return null;
        }
    }
}
