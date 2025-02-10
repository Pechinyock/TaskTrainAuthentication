using TT.Auth.Entities;
using TT.Core;

namespace TT.Auth;

public enum LoginFailedReasonEnum 
{
    UserNotFound,
    WrongPassword
}

public enum CreateFailedReasonEnum 
{
    AlreadyExists
}

public interface IUserService
{
    public Result<User, CreateFailedReasonEnum> CreateUser(UserCreateModel newUser);
    public Result<User, LoginFailedReasonEnum> Login(UserLoginModel creds);
}
