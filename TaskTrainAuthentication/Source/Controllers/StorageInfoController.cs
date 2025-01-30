using Microsoft.AspNetCore.Mvc;

namespace TT.Auth;

[ApiController]
[Route("[controller]/[action]")]
public class StorageInfoController : ControllerBase
{
    private readonly IDatabaseMetaInfoService _databaseMetaInfoService;

    public StorageInfoController(IDatabaseMetaInfoService databaseMetaInfoService)
    {
        _databaseMetaInfoService = databaseMetaInfoService;
    }

    [HttpGet]
    public string GetStorageVendorName() 
    {
        return _databaseMetaInfoService.GetDatabaseVendorName();
    }

    [HttpGet]
    public string GetCurrentDatabaseName() 
    {
        return _databaseMetaInfoService.GetCurrentDatabaseName();
    }
}
