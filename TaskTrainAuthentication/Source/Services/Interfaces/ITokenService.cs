namespace TT.Auth;

public interface ITokenService
{
    string GenerateAccessToken(string claimName);
}
