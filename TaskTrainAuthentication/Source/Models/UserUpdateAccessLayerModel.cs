using TT.Core;

namespace TT.Auth;

public class UserUpdateAccessLayerModel
{
    public string Login { get; set; }
    public UserAccesLayerEnum NewLayerValue { get; set; }
}
