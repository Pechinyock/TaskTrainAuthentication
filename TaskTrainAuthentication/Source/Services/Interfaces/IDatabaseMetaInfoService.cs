namespace TT.Auth;

public interface IDatabaseMetaInfoService
{
    public string GetDatabaseVendorName();
    public string GetCurrentDatabaseName();
}
