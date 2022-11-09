using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAppAPI.Exceptions;
using MovieAppAPI.Models.Auth;
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
        try {
            var response = await _authService.Register(registerModel);
            return Created($"~/api/user/{response.Id}", response.Token);
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
    public async Task<IActionResult> Login(UserLoginModel loginModel) {
        try {
            var token = await _authService.Login(loginModel);
            var response = new { token };
            return Ok(response);
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
        var token = AuthService.TokenFromHeader(Request.Headers);
        if (token is null) {
            return Unauthorized();
        }

        try {
            await _authService.Logout(token);
            return Ok();
        }
        catch (Exception e) {
            return BadRequest(e.Message);
        }
    }
}