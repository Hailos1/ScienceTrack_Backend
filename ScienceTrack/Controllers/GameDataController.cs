using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScienceTrack.Models;
using ScienceTrack.Repositories;

namespace ScienceTrack.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
[Authorize(Roles = "admin")]
public class GameDataController
{
    private Repository repository;

    public GameDataController(Repository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public async Task<IEnumerable<GlobalEvent>> GetGlobalEvents()
    {
        return await repository.GlobalEvents.GetList();
    }
    [HttpPost]
    public async Task UpdateGlobalEvent([FromBody] GlobalEvent globalEvent)
    {
        repository.GlobalEvents.Update(globalEvent);
        await repository.GlobalEvents.Save();
    }
    [HttpPost]
    public async Task<GlobalEvent> CreateGlobalEvent([FromBody] GlobalEvent globalEvent)
    {
        globalEvent = repository.GlobalEvents.Create(globalEvent);
        await repository.GlobalEvents.Save();
        return globalEvent;
    }
    [HttpGet]
    public async Task<IEnumerable<LocalEvent>> GetLocalEvents()
    {
        return await repository.LocalEvents.GetList();
    }
    [HttpPost]
    public async Task UpdateLocalEvent([FromBody] LocalEvent localEvent)
    {
        repository.LocalEvents.Update(localEvent);
        await repository.LocalEvents.Save();
    }
    [HttpPost]
    public async Task<LocalEvent> CreateLocalEvent([FromBody] LocalEvent localEvent)
    {
        localEvent = repository.LocalEvents.Create(localEvent);
        await repository.LocalEvents.Save();
        return localEvent;
    }
    [HttpGet]
    public async Task<IEnumerable<LocalSolution>> GetLocalSolutions()
    {
        return await repository.LocalSolutions.GetList();
    }
    [HttpPost]
    public async Task UpdateLocalSolution([FromBody] LocalSolution localSolution)
    {
        repository.LocalSolutions.Update(localSolution);
        await repository.LocalSolutions.Save();
    }
    [HttpPost]
    public async Task<LocalSolution> CreateLocalSolution([FromBody] LocalSolution localSolution)
    {
        localSolution = repository.LocalSolutions.Create(localSolution);
        await repository.LocalSolutions.Save();
        return localSolution;
    }
}