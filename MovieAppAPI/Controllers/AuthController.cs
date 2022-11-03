using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAppAPI.Exceptions;
using MovieAppAPI.Models.Auth;
using MovieAppAPI.Services;
using MovieAppAPI.Services.Auth;

namespace MovieAppAPI.Controllers;

[Route("api/account/")]
[Produces("application/json")]
[ApiController]
public class AuthController : ControllerBase {
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterModel registerModel) {
        // TODO: ask if there is way not to use the response functions
        try {
            var token = await _authService.Register(registerModel);
            var response = new { token = token };
            return new JsonResult(response);
            // return Created(Request.Path.Value ?? "", response);
        }
        catch (ObjectsAreNotEqual e) {
            return Unauthorized(e.Message);
        }
        catch (AlreadyExistsException e) {
            return Conflict(e.Message);
        }
        catch (RecordNotFoundException e) {
            return NotFound(e.Message);
        }
    }

    [HttpPost("login")]
    public IActionResult Login(UserLoginModel loginModel) {
        try {
            var token = _authService.Login(loginModel);
            var response = new { token = token };
            return new JsonResult(response);
            // return Created(Request.Path.Value ?? "", response);
        }
        catch (ObjectsAreNotEqual e) {
            return Unauthorized(e.Message);
        }
        catch (AlreadyExistsException e) {
            return Conflict(e.Message);
        }
        catch (RecordNotFoundException e) {
            return NotFound(e.Message);
        }
    }

    [HttpPost("logout")]
    [Authorize("TokenNotRejected")]
    public async Task<IActionResult> Logout() {
        var token = Request.Headers.Authorization.ToString().Split(" ")[1];
        var isLogout = await _authService.Logout(token);
        return isLogout ? Ok() : BadRequest();
    }
}