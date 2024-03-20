using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ScienceTrack.DTO;
using ScienceTrack.Models;
using ScienceTrack.Repositories;

namespace ScienceTrack.Services;

public class ImportService
{
    private Repository repository;
    private ILogger logger;
    
    public ImportService(Repository repository, ILogger<AuthorizationService> logger)
    {
        this.repository = repository;
        this.logger = logger;
    }
    
    public async Task ImportCsvUsers(IFormFile csv)
    {
        await using var fileStream = csv.OpenReadStream();
        using var reader = new StreamReader(fileStream);
        while (await reader.ReadLineAsync() is { } row)
        {
            var split = row.Split(';',',');
                
            var userName = split[0];
            var password = split[1];
            var officialName = split[2];
            var role = split.Length == 4 ? Convert.ToInt32(split[3]) : 2;
                
            if (repository.Users.GetList().Result.FirstOrDefault(x => x.UserName.ToLower() == userName.ToLower()) is null)
            {
                var user = new User()
                {
                    UserName = userName,
                    PasswordHash = password,
                    OfficialName = officialName,
                    Role = Convert.ToInt32(role)
                };

                user.PasswordHash = new PasswordHasher<User>().HashPassword(user, password);
                repository.Users.Create(user);
                await repository.Users.Save();
                logger.LogInformation("user: " + user.UserName + " was registered");
            }
        }
    }
    
    public async Task<IEnumerable<UserDTO>> GetUsers(HttpResponse response, int pageNum = 1, int pageSize = 10)
    {
        var users = repository.Users.GetQList().Include(x => x.RoleNavigation);
        var count = users.Count();
        var totalPages = (int)Math.Ceiling(count / (double)pageSize);
        response.Headers.Add("TotalPages", $"{totalPages}");
        response.Headers.Add("TotalCount", $"{count}");
        return users
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new UserDTO(x));
    }

    public async Task RemoveUser(string username, int? userId = null)
    {
        if (userId != null)
        {
            var forId = await repository.Users.GetQList().Include(x => x.RoleNavigation).FirstOrDefaultAsync(x => x.Id == userId.Value);
            if (forId.RoleNavigation.RoleName == "admin")
            {
                throw new Exception("This user is Admin");
            }
            repository.Users.Delete(userId.Value);
            await repository.Users.Save();
            return;
        }
        
        var user = await repository.Users.GetQList().Include(x => x.RoleNavigation).FirstOrDefaultAsync(x => x.UserName == username);
        if (user != null)
        {
            if (user.RoleNavigation.RoleName == "admin")
            {
                throw new Exception("This user is Admin");
            }
            
            repository.Users.Delete(user.Id);
            await repository.Users.Save();
            return;
        }

        throw new Exception("User Not Found");
    }
    
    public async Task ClearUsers()
    {
        var users = repository.Users.GetQList().Where(x => x.Role == 2).ToList();
        foreach (var user in users)
        {
            repository.Users.Delete(user.Id);
        }
        await repository.Users.Save();
    }
}