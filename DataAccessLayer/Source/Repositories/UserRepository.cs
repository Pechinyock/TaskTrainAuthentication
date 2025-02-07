using Dapper;
using Npgsql;

namespace TT.Auth.Data;

public class UserRepository
{
    private readonly string _tableName = "tt_users";
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        if(String.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        _connectionString = connectionString;
    }

    public void AddUser(Guid id, string login, string passwordHash) 
    {
        using (var connection = new NpgsqlConnection(_connectionString)) 
        {
            connection.Open();
            var cmd = $"insert into {_tableName} (id, login, password_hash) values(@id, @login, @passwordHash)";
            var queryArguments = new
            {
                Id = id,
                Login = login,
                PasswordHash = passwordHash
            };
            connection.Execute(cmd, queryArguments);
        }
    }
}
