using NUnit.Framework;
using TT.Auth.DatabaseUpdaters;
using TT.Stroage;

namespace DataAccessLayerTests;

public class DatabaseUpdaterTests
{
    private readonly string _connectionString = "Server=localhost;Port=11101;Database=postgres;Username=postgres;Password=qwerty12345;Pooling=true;Maximum Pool Size=10;Timeout=10;";

    [Test]
    public void DatabaseUpdaterInstanceCreateTest() 
    {
        ISQLDatabaseUpdater databaseUpdater = new PostgreSQLDatabaseUpdater(_connectionString);
        Assert.IsNotNull(databaseUpdater);
    }

    [Test]
    public void DatabaseUpdaterMigrationsHasBeenReleasedTest() 
    {
        ISQLDatabaseUpdater databaseUpdater = new PostgreSQLDatabaseUpdater(_connectionString);
        var migrations = databaseUpdater.GetMigrationList();
        Assert.IsNotNull(migrations);
    }
}
