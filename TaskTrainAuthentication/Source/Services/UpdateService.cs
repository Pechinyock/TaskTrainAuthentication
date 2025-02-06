using Microsoft.Extensions.Options;
using TT.Storage.Npgsql;

namespace TT.Auth;

#region option class
public class UpdaterOptions 
{
    public string DatabaseConnectionString { get; set; }
    public string MigrationsFolederPath { get; set; }
}
#endregion

public class UpdateService : IUpdateService
{
    private readonly UpdaterOptions _updaterOptions;

    public UpdateService(IOptions<UpdaterOptions> options)
    {
        _updaterOptions = options.Value;
    }

    public void DatabaseInitialize() 
    {
        var dbUpdater = new NpgsqlDatabaseUpdater(_updaterOptions.DatabaseConnectionString
            , _updaterOptions.MigrationsFolederPath
        );
        dbUpdater.Initialize();
    } 

    public void DatabaseStepBackward() 
    {
        var dbUpdater = new NpgsqlDatabaseUpdater(_updaterOptions.DatabaseConnectionString
            , _updaterOptions.MigrationsFolederPath
        );
        dbUpdater.StepBack();
    } 

    public void DatabaseStepForward() 
    {
        var dbUpdater = new NpgsqlDatabaseUpdater(_updaterOptions.DatabaseConnectionString
            , _updaterOptions.MigrationsFolederPath
        );
        dbUpdater.StepForward();
    } 

    public int GetDatabaseVersion() => throw new NotImplementedException();

    public IEnumerable<string> GetInitializeDatabaseRecipeSteps()
    {
        var dbUpdater = new NpgsqlDatabaseUpdater(_updaterOptions.DatabaseConnectionString
            , _updaterOptions.MigrationsFolederPath
        );
        return dbUpdater.GetInitializeDatabaseRecipe();
    } 

    public IEnumerable<string> GetMigrationsDownList() 
    {
        var dbUpdater = new NpgsqlDatabaseUpdater(_updaterOptions.DatabaseConnectionString
            , _updaterOptions.MigrationsFolederPath
        );
        return dbUpdater.GetMigrationsDown();
    } 

    public IEnumerable<string> GetMigrationsUpList() 
    {
        var dbUpdater = new NpgsqlDatabaseUpdater(_updaterOptions.DatabaseConnectionString
            , _updaterOptions.MigrationsFolederPath
        );
        return dbUpdater.GetMigrationsUp();
    }
}
