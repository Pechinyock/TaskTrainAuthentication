using Npgsql;
using TT.Auth.DatabaseUpdaters;
using TT.Storage;

namespace TT.Auth.DataProviders;

public class PostgreDataProvider : ISQLDataProvider
{
    private readonly string _connectionString;
    private readonly ISQLDatabaseUpdater _databaseUpdater;

    public PostgreDataProvider(string connectionString)
    {
        if (String.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        _connectionString = connectionString;
        _databaseUpdater = new PostgreSQLDatabaseUpdater(connectionString);
    }

    public string GetDefautDatabaseName()
    {
        var result = String.Empty;
        using (var connetion = new NpgsqlConnection(_connectionString))
        {
            connetion.Open();
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = connetion;
                cmd.CommandText = $"select current_database() as result;";
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result = (string)reader["result"];
                }
            }
        }
        return result;
    }

    public string GetDatabaseVendorName() => "Postgres";

}
