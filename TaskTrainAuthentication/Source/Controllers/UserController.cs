using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TT.Auth.Constants;
using TT.Core;

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
    public ActionResult Create([Required, FromBody] UserCreateModel model)
    {
        /* validate incoming model */
        /* password policy */

        var dbOperationResult = _userService.CreateUser(model);
        var result = dbOperationResult.Match<ActionResult>(success: (user) => 
        {
            return Ok();
        }, 
        failure: (reason) => 
        {
            switch (reason) 
            {
                case CreateFailedReasonEnum.AlreadyExists:
                    return Conflict($"User with login: {model.Login} already exists");
                default:
                    return BadRequest();
            }
        });

        return result;
    }

    [HttpPost]
    public ActionResult<TokenModel> Login([Required, FromBody] UserLoginModel creds)
    {
        var userOrError = _userService.Login(creds);
        var result = userOrError.Match<ActionResult<TokenModel>>(success: (user) =>
        {
            var additionalClaims = new List<Claim>()
            {
                new Claim("AccessLayer", user.AccessLayer.ToString()),
            };
            var token = _tokenService.GenerateAccessToken(user.Login, additionalClaims);
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
        return result;
    }

    [HttpPost]
    [Authorize(Policy = Policy.AccessLayer.Admin)]
    public ActionResult SetAccessLayer([Required, FromBody] UserUpdateAccessLayerModel model) 
    {
        _userService.UpdateUserAccessLayer(model);
        return Ok();
    }

    [HttpGet]
    [Authorize(Policy = Policy.AccessLayer.Admin)]
    public ActionResult All() 
    {
        return Ok();
    }
}
