using Dapper;
using Npgsql;
using TT.Auth.Entities;

namespace TT.Auth.Data;

public sealed class UserRepository : IUserRepository
{
    private readonly string _tableName = "tt_users";
    private readonly string _connectionString;

    public UserRepository(string connectionString)
    {
        if(String.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        _connectionString = connectionString;
    }

    public User AddUser(User newUser) 
    {
        Guid id = newUser.Id;
        string login = newUser.Login;
        string passwordHash = newUser.PasswordHash;

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
        return newUser;
    }

    public User GetUser(string login) 
    {
        if (String.IsNullOrEmpty(login))
            return null;

        using (var connection = new NpgsqlConnection(_connectionString)) 
        {
            connection.Open();
            var cmd = $"select * from {_tableName} where login = @login";
            var dbUser = connection.QueryFirstOrDefault(cmd, new { login = login });

            if(dbUser is null)
                return null;

            var result = new User()
            {
                Id = dbUser.id,
                Login = dbUser.login,
                PasswordHash = dbUser.password_hash,
            };
            return result;
        }
    }
}
