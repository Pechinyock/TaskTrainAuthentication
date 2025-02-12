using TT.Core;

namespace TT.Auth.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public UserAccesLayerEnum AccessLayer { get; set; }
}
