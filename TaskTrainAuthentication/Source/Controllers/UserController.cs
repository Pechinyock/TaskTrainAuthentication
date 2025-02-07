using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TT.Auth;

[ApiController]
[Route("[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userSerivce)
    {
        _userService = userSerivce;
    }

    [HttpPost]
    public ActionResult CreateUser([Required, FromBody] UserCreateModel model)
    {
        _userService.CreateUser(model.Login, model.Password);
        return Ok();
    }
}
