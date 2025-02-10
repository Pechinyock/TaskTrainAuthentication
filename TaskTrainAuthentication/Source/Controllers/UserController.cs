using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TT.Auth.Entities;

namespace TT.Auth;

/* [TODO]
 * - all services has to return ResultPattern from TT.Core
 * - all services has describe result enum at the top of class
 * - need validation at controller layer
 * - need model-entity mapper
 * - data repository inplements IRepository interface for tests (to avoid db connection)
 */

[ApiController]
[Route("[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;

    public UserController(IUserService userSerivce, ITokenService tokenService)
    {
        _userService = userSerivce;
        _tokenService = tokenService;
    }

    [HttpPost]
    public ActionResult CreateUser([Required, FromBody] UserCreateModel model)
    {
        /* validate incoming model */
        _userService.CreateUser(model);
        return Ok();
    }

    [HttpPost]
    public ActionResult<TokenModel> Login([Required, FromBody] UserLoginModel creds)
    {
        var userOrError = _userService.Login(creds);
        var resutl = userOrError.Match<ActionResult<TokenModel>>(success: (user) =>
        {
            var token = _tokenService.GenerateAccessToken(user.Login);
            return new TokenModel() { AccessToken = token };
        },
        failure: (reason) => 
        {
            switch (reason) 
            {
                case LoginFailedReasonEnum.UserNotFound:
                    return NotFound($"user with login: {creds.Login} not found");
                case LoginFailedReasonEnum.WrongPassword:
                    return Unauthorized();
            }
            return BadRequest();
        });
        return resutl;
    }
}
