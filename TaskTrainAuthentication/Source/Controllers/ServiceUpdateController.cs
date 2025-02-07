using Microsoft.AspNetCore.Mvc;

namespace TT.Auth;

[ApiController]
[Route("[controller]/[action]")]
public class ServiceUpdateController : ControllerBase
{
    private readonly IUpdateService _updateService;

    public ServiceUpdateController(IUpdateService databaseMetaInfoService)
    {
        _updateService = databaseMetaInfoService;
    }

    [HttpGet]
    public void InitializeDatabase()
    {
        _updateService.DatabaseInitialize();
    }

    [HttpGet]
    public void DatabaseStepForward() 
    {
        _updateService.DatabaseStepForward();
    }

    [HttpGet]
    public void DatabaseStepBackward() 
    {
        _updateService.DatabaseStepBackward();
    }

    [HttpGet]
    public int GetDatabaseVersion() 
    {
        return _updateService.GetDatabaseVersion();
    }

    [HttpGet]
    public string[] GetMigrationsDownList() 
    {
        return _updateService.GetMigrationsDownList().ToArray();
    }

    [HttpGet]
    public string[] GetMigrationsUpList() 
    {
        return _updateService.GetMigrationsUpList().ToArray();
    }

    [HttpGet]
    public string[] GetInitializeRecipeSteps() 
    {
        return _updateService.GetInitializeDatabaseRecipeSteps().ToArray();
    }
}
