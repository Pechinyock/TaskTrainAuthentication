using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using TT.Core;

namespace TT.Auth;

public class TokenService : ITokenService
{
    public string GenerateAccessToken(string claimName, List<Claim> appendClaims)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, claimName),
        };

        if (appendClaims != null && appendClaims.Count > 0)
            claims.AddRange(appendClaims);

        var authOptions = AuthenticationDefaults.GetDefaultOptions();
        var signedCreds = new SigningCredentials(AuthenticationDefaults.GetSymmetricKey(authOptions.Key), SecurityAlgorithms.HmacSha256);
        var jwt = new JwtSecurityToken(issuer: authOptions.Issuer
            , audience: authOptions.Audience
            , claims: claims
            , expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(authOptions.Lifetime))
            , signingCredentials: signedCreds
        );
        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
        return accessToken;
    }
}
