using Microsoft.Extensions.Options;
using TT.Auth.Data;

namespace TT.Auth;

#region option class
public class UserServiceOptions 
{
    public string ConnectionString { get; set;}
}
#endregion

public class UserService : IUserService
{
    private readonly UserServiceOptions _userServiceOptions;

    public UserService(IOptions<UserServiceOptions> options)
    {
        _userServiceOptions = options.Value;
    }

    public void CreateUser(string login, string password)
    {
        var repo = new UserRepository(_userServiceOptions.ConnectionString);
        /*hash here*/
        var passwordHash = password;
        var id = Guid.NewGuid();
        repo.AddUser(id, login, passwordHash);
    }
}
