namespace TT.Auth;

public interface IUpdateService
{
    int GetDatabaseVersion();
    IEnumerable<string> GetMigrationsUpList();
    IEnumerable<string> GetMigrationsDownList();
    IEnumerable<string> GetInitializeDatabaseRecipeSteps();
    public void DatabaseInitialize();
    public void DatabaseStepForward();
    public void DatabaseStepBackward();
}
