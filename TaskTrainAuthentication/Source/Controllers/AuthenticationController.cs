using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TT.Auth;

[ApiController]
[Route("[controller]/[action]")]
public class AuthenticationController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public AuthenticationController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpGet]
    public ActionResult<TokenModel> Login([Required] string login, [Required] string password) 
    {
        var token = _tokenService.GenerateAccessToken(login);
        return new TokenModel() { AccessToken = token };
    }
}
