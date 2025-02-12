using System.Security.Claims;

namespace TT.Auth;

public interface ITokenService
{
    string GenerateAccessToken(string claimName, List<Claim> appendCliems);
}
