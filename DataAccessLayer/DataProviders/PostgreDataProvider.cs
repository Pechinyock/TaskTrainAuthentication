using Npgsql;
using TT.Core;

namespace TT.Auth.DataProviders;

public class PostgreDataProvider : IStorageProvider
{
    private readonly string _connectionString;

    public PostgreDataProvider(string connectionString)
    {
        if (String.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        _connectionString = connectionString;
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

    StroageTypeFlags IStorageProvider.GetType() => StroageTypeFlags.Remote | StroageTypeFlags.SqlDatabase;
}
