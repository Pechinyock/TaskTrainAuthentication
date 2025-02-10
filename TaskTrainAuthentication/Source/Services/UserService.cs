using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TT.Auth.Data;
using TT.Auth.Entities;
using TT.Core;

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
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserService(IOptions<UserServiceOptions> options, IPasswordHasher<User> passwordHaser)
    {
        _userServiceOptions = options.Value;
        _passwordHasher = passwordHaser;
    }

    public Result<User, CreateFailedReasonEnum> CreateUser(UserCreateModel newUser)
    {
        var user = new User()
        {
            Id = Guid.NewGuid(),
            Login = newUser.Login,
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, newUser.Password);

        var repo = new UserRepository(_userServiceOptions.ConnectionString);

        return repo.AddUser(user);
    }

    public Result<User, LoginFailedReasonEnum> Login(UserLoginModel creds) 
    {
        var repo = new UserRepository(_userServiceOptions.ConnectionString);
        var user = repo.GetUser(creds.Login);

        if (user is null)
            return LoginFailedReasonEnum.UserNotFound;

        var passwordVerifyResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, creds.Password);

        switch (passwordVerifyResult) 
        {
            case PasswordVerificationResult.SuccessRehashNeeded:
            case PasswordVerificationResult.Success:
                return user;

            case PasswordVerificationResult.Failed:
                return LoginFailedReasonEnum.WrongPassword;
        }

        return user;
    }
}
