using Npgsql;
using TT.Stroage;

namespace TT.Auth.DatabaseUpdaters;

public sealed class PostgreSQLDatabaseUpdater : ISQLDatabaseUpdater
{
    private readonly string _connectionString;
    private readonly string _migrationFilesRealtivePath;

    private uint _databaseVersion;

    public PostgreSQLDatabaseUpdater(string connectionString)
    {
        if(String.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        _connectionString = connectionString;
        _migrationFilesRealtivePath = String.Format("{0}{1}Postgres{1}Migrations"
            , AppContext.BaseDirectory
            , Path.DirectorySeparatorChar
        );
        Initialize();
    }

    public void Update(uint version)
    {
        throw new NotImplementedException();
    }

    public void Downgrade(uint version)
    {
        throw new NotImplementedException();
    }

    public void StepBack()
    {
        throw new NotImplementedException();
    }

    public void StepForward()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> GetMigrationList()
    {
        var filePaths = Directory.GetFiles(_migrationFilesRealtivePath, "*.sql");
        return filePaths;
    }

    public string GetWorkingDatabaseName() => "TTAuth";

    public uint GetCurrentVersion() => _databaseVersion;

    private bool IsInitalized() 
    {
        try
        {
            bool result = false;
            using (var connection = new NpgsqlConnection(_connectionString)) 
            {
                connection.Open();
                using (var cmd = new NpgsqlCommand()) 
                {
                    cmd.Connection = connection;
                    cmd.CommandText = @$"select exists(
                        SELECT datname FROM pg_catalog.pg_database WHERE lower(datname) = lower('{GetWorkingDatabaseName()}')
                    );";
                    var reader = cmd.ExecuteReader();
                    while (reader.Read()) 
                    {
                        result = (bool)reader["exists"];
                    }
                }
            }
            return result;
        }
        catch
        {
            return false;
        }
    }

    private void Initialize() 
    {
        if (IsInitalized())
            return;
        var migrations = GetMigrationList().ToArray();
        if (!migrations.Any())
            throw new InvalidOperationException("Couldn't find migrations files");

        using (var connection = new NpgsqlConnection(_connectionString)) 
        {
            connection.Open();
            using (var cmd = new NpgsqlCommand()) 
            {
                cmd.Connection = connection;

                for (int i = 0; i < migrations.Length; ++i) 
                {
                    var sqlText = File.ReadAllText(migrations[i]);
                    if (String.IsNullOrEmpty(sqlText))
                        continue;

                    cmd.CommandText = sqlText;
                    cmd.ExecuteScalar();
                }

            }
        }
    }
}
