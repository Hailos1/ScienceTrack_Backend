using Microsoft.AspNetCore.Identity;
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