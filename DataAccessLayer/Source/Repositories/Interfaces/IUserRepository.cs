using TT.Auth.Entities;

namespace TT.Auth.Data;

public interface IUserRepository
{
    User AddUser(User newUser);
    User GetUser(string login);
}
